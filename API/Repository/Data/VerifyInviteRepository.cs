using API.Context;
using API.Models;
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
            var verifyId = myContext.VerifyInvites.FirstOrDefault(e => e.UserId == verifyInvite.UserId && e.BoardID == verifyInvite.BoardID).Id;

            if (checkUserBoard)
            {
                var isUsed = myContext.VerifyInvites.SingleOrDefault(e => e.Id == verifyId).IsUsed;
                var isAccept = myContext.VerifyInvites.SingleOrDefault(e => e.Id == verifyId).IsAccept;
                if (isAccept)
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int AcceptInvite(VerifyInvite verifyInvite)
        {
            var regMemberBoard = new MemberBoard
            {
                UserId = verifyInvite.UserId,
                BoardId = verifyInvite.BoardID,
                Role = "Bussiness Analyst"
            };

            myContext.MemberBoards.Add(regMemberBoard);
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
