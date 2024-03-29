﻿using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;

namespace API.Repository.Data
{
    public class BoardRepository : GeneralRepository<MyContext, Board, string>
    {
        private MemberBoardRepository memberBoardRepository;
        public BoardRepository(MyContext myContext, MemberBoardRepository memberBoardRepository) : base(myContext)
        {
            this.myContext = myContext;
            this.memberBoardRepository = memberBoardRepository;
        }
        public object CreateBoard(Board board)
        {
            board.Id = Guid.NewGuid().ToString();
            myContext.Boards.Add(board);
            myContext.SaveChanges();

            var addMemberBoard = new MemberBoard
            {
                UserId = board.CreatedBy,
                BoardId = board.Id,
                Role = "PM"
            };

            myContext.MemberBoards.Add(addMemberBoard);
            myContext.SaveChanges();
            var getBoard = GetBoardById(board.Id);

            if (getBoard == null) throw new Exception("Error occured.");

            return getBoard;
        }

        public IEnumerable GetBoardByMember2(int id)
        {

            var board = myContext.Boards.Where(x => x.MemberBoards.Any(y => y.UserId == id)).AsSplitQuery()
                           .Include(x => x.Lists.OrderByDescending(y => y.CreatedAt))
                           .ThenInclude(x => x.Cards.OrderByDescending(y => y.CreatedAt))
                           .Include(x => x.VerifyInvites)
                           .Include(x => x.MemberBoards)
                           .ThenInclude(x => x.User)
                           .Include(x => x.MemberBoards)
                           .ToList();
            return board;
        }

        public IEnumerable GetBoardByMember(int Id)
        {
            var board = (from memberboard in myContext.MemberBoards
                         join brd in myContext.Boards on memberboard.BoardId equals brd.Id
                         where memberboard.UserId == Id
                         let createdBy = (from cb_user in myContext.Users
                                          where cb_user.Id == brd.CreatedBy
                                          select new
                                          {
                                              id = cb_user.Id,
                                              fullName = cb_user.FullName,
                                              email = cb_user.Email,
                                              gender = cb_user.Gender,
                                              image = cb_user.Image
                                          }).ToList()

                         let members = (from m_user in myContext.Users
                                        join m_mb in myContext.MemberBoards on m_user.Id equals m_mb.UserId
                                        where m_mb.BoardId == brd.Id
                                        select new
                                        {
                                            id = m_user.Id,
                                            fullName = m_user.FullName,
                                            email = m_user.Email,
                                            gender = m_user.Gender,
                                            image = m_user.Image,
                                            role = m_mb.Role
                                        }).ToList()

                         let lists = (from list in myContext.Lists
                                      where list.BoardId == brd.Id
                                      select new
                                      {
                                          id = list.Id,
                                          name = list.Name,
                                          createdAt = list.CreatedAt,
                                          status = list.Status,
                                          createdBy = (from l_user in myContext.Users
                                                       where l_user.Id == list.CreatedBy
                                                       select new
                                                       {
                                                           id = l_user.Id,
                                                           fullName = l_user.FullName,
                                                           email = l_user.Email,
                                                           gender = l_user.Gender,
                                                           image = l_user.Image
                                                       }).ToList(),

                                          cards = (from card in myContext.Cards
                                                   where card.ListId == list.Id
                                                   select new
                                                   {
                                                       id = card.Id,
                                                       name = card.Name,
                                                       description = card.Status,
                                                       createdAt = card.CreatedAt,
                                                       createdBy = (from c_user in myContext.Users
                                                                    where c_user.Id == card.CreatedBy
                                                                    select new
                                                                    {
                                                                        id = c_user.Id,
                                                                        fullName = c_user.FullName,
                                                                        email = c_user.Email,
                                                                        gender = c_user.Gender,
                                                                        image = c_user.Image
                                                                    }).ToList(),
                                                       checklist = (from check in myContext.CheckListItems
                                                                    where check.CardId == card.Id
                                                                    select new
                                                                    {
                                                                        id = check.Id,
                                                                        name = check.Name,
                                                                        due = check.Due,
                                                                        assign = (from cl in myContext.CheckListItemsAssigns
                                                                                  join cl_user in myContext.Users on cl.UserId equals cl_user.Id
                                                                                  where cl.CheckListItemId == check.Id
                                                                                  select new
                                                                                  {
                                                                                      id = cl_user.Id,
                                                                                      fullName = cl_user.FullName,
                                                                                      email = cl_user.Email,
                                                                                      gender = cl_user.Gender,
                                                                                      image = cl_user.Image
                                                                                  }).ToList()
                                                                    }).ToList(),
                                                       comments = (from comment in myContext.Comments
                                                                   where comment.CardId == card.Id
                                                                   select new
                                                                   {
                                                                       id = comment.Id,
                                                                       user = (from com in myContext.Comments
                                                                               join com_user in myContext.Users on com.UserId equals com_user.Id
                                                                               where com.CardId == card.Id
                                                                               select new
                                                                               {
                                                                                   id = com_user.Id,
                                                                                   fullName = com_user.FullName,
                                                                                   email = com_user.Email,
                                                                                   gender = com_user.Gender,
                                                                                   image = com_user.Image
                                                                               }).ToList(),
                                                                       comment = comment.Text,
                                                                       createdAt = comment.CreatedAt
                                                                   }).ToList()
                                                   }).ToList()
                                      }).ToList()
                         select new
                         {
                             id = brd.Id,
                             name = brd.Name,
                             description = brd.Description,
                             createdBy = createdBy,
                             createAt = brd.CreatedAt,
                             members = members,
                             lists = lists
                         }).ToList();
            return board;
        }

        public IEnumerable GetBoardByCreator(int Id)
        {
            var board = (from brd in myContext.Boards
                         where brd.CreatedBy == Id
                         let createdBy = (from cb_user in myContext.Users
                                          where cb_user.Id == Id
                                          select new
                                          {
                                              fullName = cb_user.FullName,
                                              email = cb_user.Email,
                                              gender = cb_user.Gender,
                                              image = cb_user.Image
                                          }).ToList()

                         let members = (from m_user in myContext.Users
                                        join m_mb in myContext.MemberBoards on m_user.Id equals m_mb.UserId
                                        where m_mb.BoardId == brd.Id
                                        select new
                                        {
                                            fullName = m_user.FullName,
                                            email = m_user.Email,
                                            gender = m_user.Gender,
                                            image = m_user.Image
                                        }).ToList()

                         let lists = (from list in myContext.Lists
                                      where list.BoardId == brd.Id
                                      select new
                                      {
                                          name = list.Name,
                                          createdAt = list.CreatedAt,
                                          status = list.Status,
                                          createdBy = (from l_user in myContext.Users
                                                       where l_user.Id == list.CreatedBy
                                                       select new
                                                       {
                                                           fullName = l_user.FullName,
                                                           email = l_user.Email,
                                                           gender = l_user.Gender,
                                                           image = l_user.Image
                                                       }).ToList(),

                                          cards = (from card in myContext.Cards
                                                   where card.ListId == list.Id
                                                   select new
                                                   {
                                                       name = card.Name,
                                                       description = card.Status,
                                                       createdAt = card.CreatedAt,
                                                       createdBy = (from c_user in myContext.Users
                                                                    where c_user.Id == card.CreatedBy
                                                                    select new
                                                                    {
                                                                        fullName = c_user.FullName,
                                                                        email = c_user.Email,
                                                                        gender = c_user.Gender,
                                                                        image = c_user.Image
                                                                    }).ToList(),
                                                       checklist = (from check in myContext.CheckListItems
                                                                    where check.CardId == card.Id
                                                                    select new
                                                                    {
                                                                        name = check.Name,
                                                                        due = check.Due,
                                                                        assign = (from cl in myContext.CheckListItemsAssigns
                                                                                  join cl_user in myContext.Users on cl.UserId equals cl_user.Id
                                                                                  where cl.CheckListItemId == check.Id
                                                                                  select new
                                                                                  {
                                                                                      fullName = cl_user.FullName,
                                                                                      email = cl_user.Email,
                                                                                      gender = cl_user.Gender,
                                                                                      image = cl_user.Image
                                                                                  }).ToList()
                                                                    }).ToList(),
                                                       comments = (from comment in myContext.Comments
                                                                   where comment.CardId == card.Id
                                                                   select new
                                                                   {
                                                                       user = (from com in myContext.Comments
                                                                               join com_user in myContext.Users on com.UserId equals com_user.Id
                                                                               where com.CardId == card.Id
                                                                               select new
                                                                               {
                                                                                   fullName = com_user.FullName,
                                                                                   email = com_user.Email,
                                                                                   gender = com_user.Gender,
                                                                                   image = com_user.Image
                                                                               }).ToList(),
                                                                       comment = comment.Text,
                                                                       createdAt = comment.CreatedAt
                                                                   }).ToList()
                                                   }).ToList()
                                      }).ToList()
                         select new
                         {
                             name = brd.Name,
                             description = brd.Description,
                             createdBy = createdBy,
                             createAt = brd.CreatedAt,
                             members = members,
                             lists = lists
                         }).ToList();
            return board;
        }

        public object GetBoardDetailById(string id)
        {
            //  var board = myContext.Boards.Find(id).Include()
            var board = myContext.Boards
                           .Include(m => m.MemberBoards).ThenInclude(us => us.User)
                           .Include(l => l.Lists)
                           .ThenInclude(c => c.Cards).ThenInclude(m => m.MemberCards).ThenInclude(us => us.User)
                           .Include(l => l.Lists)
                           .ThenInclude(c => c.Cards).ThenInclude(c => c.CheckListItems).ThenInclude(m => m.CheckListItemAssigns).ThenInclude(us => us.User)
                           .OrderBy(a => a.CreatedAt)
                           .AsSplitQuery().FirstOrDefault(col => col.Id == id);

            if (board == null) throw new Exception("Board not found");

            return board;
        }

        public object GetBoardById(string Id)
        {
            var board = (from brd in myContext.Boards
                         where brd.Id == Id
                         let createdBy = (from cb_user in myContext.Users
                                          where cb_user.Id == brd.CreatedBy
                                          select new
                                          {
                                              id = cb_user.Id,
                                              fullName = cb_user.FullName,
                                              email = cb_user.Email,
                                              gender = cb_user.Gender,
                                              image = cb_user.Image
                                          }).ToList()

                         let members = (from m_user in myContext.Users
                                        join m_mb in myContext.MemberBoards on m_user.Id equals m_mb.UserId
                                        where m_mb.BoardId == brd.Id
                                        select new
                                        {
                                            fullName = m_user.FullName,
                                            email = m_user.Email,
                                            gender = m_user.Gender,
                                            image = m_user.Image,
                                            role = m_mb.Role
                                        }).ToList()

                         let lists = (from list in myContext.Lists
                                      where list.BoardId == brd.Id
                                      orderby list.CreatedAt ascending
                                      select new
                                      {
                                          id = list.Id,
                                          boardId = list.BoardId,
                                          name = list.Name,
                                          createdAt = list.CreatedAt,
                                          status = list.Status,
                                          createdBy = (from l_user in myContext.Users
                                                       where l_user.Id == list.CreatedBy
                                                       select new
                                                       {
                                                           id = l_user.Id,
                                                           fullName = l_user.FullName,
                                                           email = l_user.Email,
                                                           gender = l_user.Gender,
                                                           image = l_user.Image
                                                       }).ToList(),

                                          cards = (from card in myContext.Cards
                                                   where card.ListId == list.Id
                                                   select new
                                                   {
                                                       name = card.Name,
                                                       description = card.Status,
                                                       createdAt = card.CreatedAt,
                                                       createdBy = (from c_user in myContext.Users
                                                                    where c_user.Id == card.CreatedBy
                                                                    select new
                                                                    {
                                                                        fullName = c_user.FullName,
                                                                        email = c_user.Email,
                                                                        gender = c_user.Gender,
                                                                        image = c_user.Image
                                                                    }).ToList(),
                                                       checklist = (from check in myContext.CheckListItems
                                                                    where check.CardId == card.Id
                                                                    select new
                                                                    {
                                                                        name = check.Name,
                                                                        due = check.Due,
                                                                        assign = (from cl in myContext.CheckListItemsAssigns
                                                                                  join cl_user in myContext.Users on cl.UserId equals cl_user.Id
                                                                                  where cl.CheckListItemId == check.Id
                                                                                  select new
                                                                                  {
                                                                                      fullName = cl_user.FullName,
                                                                                      email = cl_user.Email,
                                                                                      gender = cl_user.Gender,
                                                                                      image = cl_user.Image
                                                                                  }).ToList()
                                                                    }).ToList(),
                                                       comments = (from comment in myContext.Comments
                                                                   where comment.CardId == card.Id
                                                                   select new
                                                                   {
                                                                       user = (from com_user in myContext.Users
                                                                               where com_user.Id == comment.UserId
                                                                               select new
                                                                               {
                                                                                   fullName = com_user.FullName,
                                                                                   email = com_user.Email,
                                                                                   gender = com_user.Gender,
                                                                                   image = com_user.Image
                                                                               }).ToList(),
                                                                       comment = comment.Text,
                                                                       createdAt = comment.CreatedAt
                                                                   }).ToList()
                                                   }).ToList()
                                      }).ToList()
                         select new
                         {
                             name = brd.Name,
                             description = brd.Description,
                             createdBy = createdBy,
                             createAt = brd.CreatedAt,
                             members = members,
                             lists = lists
                         });
            return board.FirstOrDefault();
        }

        public bool AuthenticateMember(string token, string BoardId)
        {
            var handler = new JwtSecurityTokenHandler();

            var decode = handler.ReadJwtToken(token);

            var id = decode.Claims.First(claim => claim.Type == "Id").Value;
            var role = decode.Claims.First(claim => claim.Type == "Roles").Value;
            var fullName = decode.Claims.First(claim => claim.Type == "Fullname").Value;
            var image = decode.Claims.First(claim => claim.Type == "Image").Value;
            var email = decode.Claims.First(claim => claim.Type == "Email").Value;

            var memberBoard = memberBoardRepository.GetMemberByBoard(BoardId).Cast<object>().ToList();
            //var memberBoard = memberBoardRepository.GetMemberByBoard(BoardId);
            var idList = new List<int>();

            foreach (object member in memberBoard)
            {
                PropertyInfo type = member.GetType().GetProperty("UserId");
                int userId = (int)(type.GetValue(member, null));
                idList.Add(userId);
            }
            if (idList.Contains(Int32.Parse(id)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
