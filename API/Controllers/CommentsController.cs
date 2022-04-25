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
    public class CommentsController : BaseController<Comment, CommentRepository, string>
    {
        public CommentsController(CommentRepository CommentRepository, MyContext myContext) : base(CommentRepository)
        {
           
        }
    }
}
