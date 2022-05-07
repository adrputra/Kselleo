using System.Collections.Generic;
using System.Linq;
using API.Context;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
   public class CheckListItemRepository : GeneralRepository<MyContext, CheckListItem, int>

   {
      public CheckListItemRepository(MyContext myContext) : base(myContext)
      {
         this.myContext = myContext;
      }

      // get checklistitems by checklistid
      public CheckListItem GetDetailChecklistItem(int checklistId)
      {
         return myContext.CheckListItems.Where(x => x.Id == checklistId)
                .Include(x => x.CheckListItemAssigns)
                .ThenInclude(x => x.User)
                .OrderByDescending(x => x.Id)
                .AsSplitQuery().FirstOrDefault();
      }


   }
}
