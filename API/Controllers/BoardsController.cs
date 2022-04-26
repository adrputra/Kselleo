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
    public class BoardsController : BaseController<Board, BoardRepository, int>
    {
        public BoardsController(BoardRepository BoardRepository, MyContext myContext) : base(BoardRepository)
        {
           
        }
    }
}
