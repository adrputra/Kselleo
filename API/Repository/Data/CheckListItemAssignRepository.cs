using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class CheckListItemAssignRepository : GeneralRepository<MyContext, CheckListItemAssign, string>

    {
        public CheckListItemAssignRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
