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
        public MemberBoardsController(MemberBoardRepository MemberBoardRepository, MyContext myContext) : base(MemberBoardRepository)
        {
           
        }
    }
}
