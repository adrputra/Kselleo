using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, int>

    {
        public AccountRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
