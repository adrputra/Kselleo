using System.Linq;
using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
   public class CardRepository : GeneralRepository<MyContext, Card, string>

   {
      public CardRepository(MyContext myContext) : base(myContext)
      {
         this.myContext = myContext;
      }

      // find card by id include user
      public Card DetailCard(int id)
      {
         return myContext.Cards
             .Include(list => list.List)
             .Include(comment => comment.Comments)
             .Include(check => check.CheckListItems)
             .AsSplitQuery()
             .OrderByDescending(x => x.CreatedAt)
             .FirstOrDefault(x => x.Id == id);
      }
   }
}
