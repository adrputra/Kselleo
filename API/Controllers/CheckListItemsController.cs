using System;
using System.Net;
using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class CheckListItemsController : BaseController<CheckListItem, CheckListItemRepository, int>
   {
      CheckListItemRepository checkListItemRepository;

      public CheckListItemsController(CheckListItemRepository CheckListItemRepository, MyContext myContext) : base(CheckListItemRepository)
      {
         this.checkListItemRepository = CheckListItemRepository;
      }

      [HttpGet("detail/{Id}")]
      public ActionResult GetCard(int Id)
      {
         try
         {
            var card = checkListItemRepository.GetDetailChecklistItem(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get ChecklistItem {Id} Successfully!", data = card });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
      }


   }
}
