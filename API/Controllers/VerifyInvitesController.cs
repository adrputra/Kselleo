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
    public class VerifyInvitesController : BaseController<VerifyInvite, VerifyInviteRepository, int>
    {
        public VerifyInvitesController(VerifyInviteRepository verifyInviteRepository, MyContext myContext) : base(verifyInviteRepository)
        {
           
        }
    }
}
