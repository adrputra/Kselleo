using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class MemberCardRepository : GeneralRepository<MyContext, MemberCard, string>

    {
        public MemberCardRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
