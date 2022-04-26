using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class VerifyInviteRepository : GeneralRepository<MyContext, VerifyInvite, int>

    {
        public VerifyInviteRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int VerifyInvite(VerifyInvite)
        {

        }
    }
}
