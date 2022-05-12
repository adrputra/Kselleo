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
   public class CommentsController : BaseController<Comment, CommentRepository, int>
   {
        public CommentRepository commentRepository;
      public CommentsController(CommentRepository CommentRepository, MyContext myContext) : base(CommentRepository)
      {
            this.commentRepository = CommentRepository;
      }

    [HttpPost("send")]
    public ActionResult SendComment(Comment comment)
        {
            try
            {
                var result = commentRepository.SendComment(comment);
                return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Comment By Id {comment.Id} Successfully!", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }
   }
}
