using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Repository.Data
{
   public class CommentRepository : GeneralRepository<MyContext, Comment, int>

   {
      public CommentRepository(MyContext myContext) : base(myContext)
      {
            this.myContext = myContext;
      }

        public object SendComment(Comment comment)
        {
            myContext.Comments.Add(comment);
            myContext.SaveChanges();

            return myContext.Comments
                .Include(u => u.User)
                .AsSplitQuery()
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault(x => x.Id == comment.Id);
        }

    }
}
