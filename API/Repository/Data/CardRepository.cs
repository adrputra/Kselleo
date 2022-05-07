﻿using System;
using System.Linq;
using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Repository.Data
{
   public class CardRepository : GeneralRepository<MyContext, Card, int>

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
             .ThenInclude(assign => assign.CheckListItemAssigns)
             .ThenInclude(user => user.User)
             .ThenInclude(user => user.MemberBoards)
             .AsSplitQuery()
             .OrderByDescending(x => x.CreatedAt)
             .FirstOrDefault(x => x.Id == id);
      }

      // create checkListItem and checkListItemAssign
      public void CreateTask(CreateTaskVM createTaskVM)
      {
         // create checkListItem
         var checkListItem = new CheckListItem
         {
            Name = createTaskVM.Name,
            CardId = createTaskVM.CardId,
            Due = createTaskVM.Due
         };
         var haha = myContext.CheckListItems.Add(checkListItem);
         myContext.SaveChanges();

         // create checkListItemAssign
         foreach (var member in createTaskVM.Members)
         {
            CheckListItemAssign checkListItemAssign = new CheckListItemAssign
            {
               CheckListItemId = checkListItem.Id,
               UserId = member
            };
            myContext.CheckListItemsAssigns.Add(checkListItemAssign);
         }
         myContext.SaveChanges();
      }

      public void UpdateTask(UpdateTaskVM updateTask)
      {
         // find checklistitems
         var checkListItem = myContext.CheckListItems.FirstOrDefault(x => x.Id == updateTask.Id);
         // update checklistitems
         checkListItem.Name = updateTask.Name;
         checkListItem.CardId = updateTask.CardId;
         checkListItem.Due = updateTask.Due;
         // update checklistitemsassign
         var checkListItemAssigns = myContext.CheckListItemsAssigns.Where(x => x.CheckListItemId == updateTask.Id);
         foreach (var checkListItemAssign in checkListItemAssigns)
         {
            myContext.CheckListItemsAssigns.Remove(checkListItemAssign);
         }
         foreach (var member in updateTask.Members)
         {
            CheckListItemAssign checkListItemAssign = new CheckListItemAssign
            {
               CheckListItemId = updateTask.Id,
               UserId = member
            };
            myContext.CheckListItemsAssigns.Add(checkListItemAssign);
         }
         myContext.SaveChanges();
      }

      // checked task
      public void CheckTask(CheckTaskVM checkTask)
      {
         try
         {
            // find checklistitems
            var checkListItem = myContext.CheckListItems.FirstOrDefault(x => x.Id == checkTask.Id);
            if (checkListItem == null) throw new Exception("Checklist item not found");

            // update checklistitems
            checkListItem.IsChecked = !checkTask.IsChecked;
            myContext.SaveChanges();
         }
         catch (System.Exception ex)
         {
            throw new Exception(ex.Message);
         }
      }
   }
}
