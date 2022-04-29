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
   public class MemberBoardsController : BaseController<MemberBoard, MemberBoardRepository, int>
   {
      private MemberBoardRepository memberBoardRepository;

      public MemberBoardsController(MemberBoardRepository MemberBoardRepository, MyContext myContext) : base(MemberBoardRepository)
      {
         memberBoardRepository = MemberBoardRepository;
      }

      [HttpGet("board/{Id}")]
      public ActionResult GetBoard(int Id)
      {
         try
         {
            var getMember = memberBoardRepository.GetMemberByBoard(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Member Board By ID Board {Id} Successfully!", data = getMember });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
      }

      [HttpDelete("delete/{id}")]
      public ActionResult DeleteCustom(int id)
      {
         try
         {
            var delete = memberBoardRepository.DeleteMember(id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Delete Member Board By ID {id} Successfully!" });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
      }
   }
}
