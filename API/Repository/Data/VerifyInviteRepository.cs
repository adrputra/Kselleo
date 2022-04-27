using API.Context;
using API.Models;
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

        public int VerifyInvite(VerifyInvite verifyInvite)
        {
            var checkUserBoard = myContext.VerifyInvites.Any(e => e.UserId == verifyInvite.UserId && e.BoardID == verifyInvite.BoardID);

            if (checkUserBoard)
            {
                var verifyId = myContext.VerifyInvites.FirstOrDefault(e => e.UserId == verifyInvite.UserId && e.BoardID == verifyInvite.BoardID).Id;
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
                            UserId = verifyInvite.UserId,
                            BoardID = verifyInvite.BoardID,
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
                    UserId = verifyInvite.UserId,
                    BoardID = verifyInvite.BoardID,
                    IsAccept = false,
                    IsUsed = false
                };
                myContext.VerifyInvites.Add(addVerifyInvite);
                myContext.SaveChanges();
                return 0;
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
