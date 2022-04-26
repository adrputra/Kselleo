using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>

    {
        public AccountRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
