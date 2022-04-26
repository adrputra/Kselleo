using API.Context;
using API.Models;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
    public class CommentRepository : GeneralRepository<MyContext, Comment, string>

    {
        public CommentRepository(MyContext myContext) : base(myContext)
        {
        }


    }
}
