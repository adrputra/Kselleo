using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class UserRepository : GeneralRepository<MyContext, User, string>

    {
        public UserRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
