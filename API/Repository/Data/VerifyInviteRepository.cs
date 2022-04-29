using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Linq;

namespace API.Repository.Data
{
   public class VerifyInviteRepository : GeneralRepository<MyContext, VerifyInvite, int>

   {
      public VerifyInviteRepository(MyContext myContext) : base(myContext)
      {
         this.myContext = myContext;
      }

      public int InviteMember(InviteMemberVM inviteMemberVM)
      {
         var user = myContext.Users.SingleOrDefault(e => e.Email == inviteMemberVM.Email);


         if (user != null)
         {
            var checkUserBoard = myContext.VerifyInvites.Any(e => e.UserId == user.Id && e.BoardID == inviteMemberVM.BoardId);
            if (checkUserBoard)
            {
               var verifyId = myContext.VerifyInvites.FirstOrDefault(e => e.UserId == user.Id && e.BoardID == inviteMemberVM.BoardId).Id;
               var isUsed = myContext.VerifyInvites.SingleOrDefault(e => e.Id == verifyId).IsUsed;
               var isAccept = myContext.VerifyInvites.SingleOrDefault(e => e.Id == verifyId).IsAccept;
               if (isUsed)
               {
                  if (isAccept)
                  {
                     return 2;
                  }
                  else
                  {
                     var addVerifyInvite = new VerifyInvite
                     {
                        UserId = user.Id,
                        BoardID = inviteMemberVM.BoardId,
                        IsAccept = false,
                        IsUsed = false,
                        Role = inviteMemberVM.Role
                     };
                     myContext.VerifyInvites.Add(addVerifyInvite);
                     myContext.SaveChanges();
                     return 0;
                  }
               }
               return 1;
            }
            else
            {
               var addVerifyInvite = new VerifyInvite
               {
                  UserId = user.Id,
                  BoardID = inviteMemberVM.BoardId,
                  IsAccept = false,
                  IsUsed = false,
                  Role = inviteMemberVM.Role

               };
               myContext.VerifyInvites.Add(addVerifyInvite);
               myContext.SaveChanges();
               return 0;
            }
         }
         else
         {
            return 3;
         }

      }

      public int AcceptInvite(VerifyInvite verifyInvite)
      {
         myContext.Entry(verifyInvite).State = EntityState.Modified;

         Console.WriteLine(verifyInvite.UserId);
         Console.WriteLine(verifyInvite.BoardID);
         Console.WriteLine(verifyInvite.Role);

         if (verifyInvite.IsAccept == true)
         {
            var regMemberBoard = new MemberBoard
            {
               UserId = verifyInvite.UserId,
               BoardId = verifyInvite.BoardID,
               Role = verifyInvite.Role
            };
            myContext.MemberBoards.Add(regMemberBoard);
         }

         if (myContext.SaveChanges() != 0)
         {
            return 0;
         }
         else
         {
            return 1;
         }
      }
      public IEnumerable PendingInvitation(int Id)
      {
         //var result = myContext.VerifyInvites.Where(e => e.UserId == Id && e.IsUsed == false);

         var result = (from ver in myContext.VerifyInvites
                       join brd in myContext.Boards on ver.BoardID equals brd.Id
                       join user in myContext.Users on brd.CreatedBy equals user.Id
                       where ver.UserId == Id
                       where ver.IsUsed == false
                       select new
                       {
                          id = ver.Id,
                          userId = ver.UserId,
                          boardId = ver.BoardID,
                          isAccept = ver.IsAccept,
                          isUsed = ver.IsUsed,
                          PM = user.FullName,
                          boardName = brd.Name,
                          role = ver.Role
                       }).ToList();
         return result;
      }

      public IEnumerable PendingInvitationBoard(int Id)
      {
         var pending = myContext.VerifyInvites
                           .Where(vi => vi.BoardID == Id && vi.IsUsed == false)
                           .Include(vi => vi.User)
                           .Include(vi => vi.Board)
                           .ToList();

         return pending;
      }
   }


}
