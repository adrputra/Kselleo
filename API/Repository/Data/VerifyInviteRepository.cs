using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            var checkUserBoard = myContext.VerifyInvites.Any(e => e.UserId == user.Id && e.BoardID == inviteMemberVM.BoardId);

            if (user != null)
            {
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
                                IsUsed = false
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
                        IsUsed = false
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
            if (verifyInvite.IsAccept == true)
            {
                var regMemberBoard = new MemberBoard
                {
                    UserId = verifyInvite.UserId,
                    BoardId = verifyInvite.BoardID,
                    Role = "Bussiness Analyst"
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
    }
}
