using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class ListRepository : GeneralRepository<MyContext, List, int>

    {
        public ListRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
