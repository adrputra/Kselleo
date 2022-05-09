using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class BoardsController : BaseController<Board, BoardRepository, string>
   {
      private BoardRepository boardRepository;
      public BoardsController(BoardRepository boardRepository, MyContext myContext) : base(boardRepository)
      {
         this.boardRepository = boardRepository;
      }

      [HttpGet("user/{Id}")]
      public ActionResult GetBoard(int Id)
      {
         try
         {
            // var board = boardRepository.GetBoardByMember(Id);
            var board = boardRepository.GetBoardByMember2(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Board By User {Id} Successfully!", data = board });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
         //return Ok(_accountRepository.GetMaster(NIK));
      }

      [HttpGet("creator/{Id}")]
      public ActionResult GetBoardCreatedBy(int Id)
      {
         try
         {
            var board = boardRepository.GetBoardByCreator(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Board By User {Id} Successfully!", data = board });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
         //return Ok(_accountRepository.GetMaster(NIK));
      }

      [HttpPost("create")]
      public ActionResult CreateBoard(Board board)
      {
         try
         {
            var result = boardRepository.CreateBoard(board);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = "Board created successfully", data = result });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
         //return Ok(boardRepository.CreateBoard(board));
      }

      [HttpGet("detail/{Id}")]
      public ActionResult GetBoardDetailById(string Id)
      {
         try
         {
            var board = boardRepository.GetBoardDetailById(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Board By Id {Id} Successfully!", data = board });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
      }

   }
}
