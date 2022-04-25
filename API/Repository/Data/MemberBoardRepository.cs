using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class MemberBoardRepository : GeneralRepository<MyContext, MemberBoard, int>

    {
        public MemberBoardRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
