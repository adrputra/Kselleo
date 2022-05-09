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

        public void DeleteCheckListItem(int checklistId)
        {
            var assign = myContext.CheckListItemsAssigns.Where(x => x.CheckListItemId == checklistId);
            var cardId = myContext.CheckListItems.FirstOrDefault(x => x.Id == checklistId).CardId;
            var userList = new List<int>();
            foreach (var item in assign)
            {
                userList.Add(item.UserId);
            }

            foreach (var item in userList)
            {
                var memberCard = myContext.MemberCards.FirstOrDefault(x => x.CardId == cardId && x.UserId == item);
                myContext.MemberCards.Remove(memberCard);
            }
            myContext.Remove(myContext.CheckListItems.FirstOrDefault(x => x.Id == checklistId));
            myContext.SaveChanges();
        }


   }
}
