using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class BoardRepository : GeneralRepository<MyContext, Board, int>

    {
        public BoardRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
