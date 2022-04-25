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
    public class MemberCardsController : BaseController<MemberCard, MemberCardRepository, string>
    {
        public MemberCardsController(MemberCardRepository MemberCardRepository, MyContext myContext) : base(MemberCardRepository)
        {
           
        }
    }
}
