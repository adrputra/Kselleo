using API.Base;
using API.Context;
using API.Models;
using API.ViewModel;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyInvitesController : BaseController<VerifyInvite, VerifyInviteRepository, int>
    {
        public VerifyInviteRepository verifyInviteRepository;
        public VerifyInvitesController(VerifyInviteRepository verifyInviteRepository, MyContext myContext) : base(verifyInviteRepository)
        {
            this.myContext = myContext;
            this.verifyInviteRepository = verifyInviteRepository;
        }

        [HttpPost("verify")]
        public ActionResult Invite(InviteMemberVM inviteMemberVM)
        {
            try
            {
                int verify = verifyInviteRepository.InviteMember(inviteMemberVM);
                return verify switch
                {
                    0 => Ok(new { code = HttpStatusCode.OK, message = "Invite Member Successfull" }),
                    1 => BadRequest(new { code = HttpStatusCode.BadRequest, message = $"Invite Member Failed, User {inviteMemberVM.Email} is already invited to Board ID {inviteMemberVM.BoardId}! (Pending Invitation)" }),
                    2 => BadRequest(new { code = HttpStatusCode.BadRequest, message = $"Invite Member Failed, User {inviteMemberVM.Email} is already joined Board ID {inviteMemberVM.BoardId}!" }),
                    3 => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Email not found" }),
                    _ => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Invite Member Failed!" })
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        [HttpPut("accept")]
        public ActionResult Accept(VerifyInvite verifyInvite)
        {
            try
            {
                int accept = verifyInviteRepository.AcceptInvite(verifyInvite);
                return accept switch
                {
                    0 => Ok(new { code = HttpStatusCode.OK, message = "Successfully Accept Invitation" }),
                    1 => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Invitation Accept Failed!" }),
                    _ => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Failed!" })
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }
    }
}
