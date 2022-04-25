using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class CheckListItemRepository : GeneralRepository<MyContext, CheckListItem, string>

    {
        public CheckListItemRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
