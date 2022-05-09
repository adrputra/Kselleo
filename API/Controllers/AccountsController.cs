using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, int>
    {
        public AccountRepository accountRepository;

        public AccountsController(AccountRepository accountRepository, MyContext myContext) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this.myContext = myContext;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            //return Ok(_accountRepository.Register(registerVM));
            try
            {
                int register = accountRepository.Register(registerVM);
                return register switch
                {
                    0 => Ok(new { code = HttpStatusCode.OK, message = "Register Successfull" }),
                    1 => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Register Failed, Email is already used!" }),
                    _ => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Register Failed!" })
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        [HttpPost("login")]
        public ActionResult Login(LoginVM loginVM)
        {
            //var login = _accountRepository.Login(loginVM);
            //return Ok(login);
            try
            {
                var login = accountRepository.Login(loginVM);
                return login switch
                {
                    "1" => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Login Failed. Wrong Password!" }),
                    "2" => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Login Failed. Email Not Found!" }),
                    "3" => BadRequest(new { code = HttpStatusCode.BadRequest, message = "Login Failed. Email Found But No Account!" }),
                    _ => Ok(new { code = HttpStatusCode.OK, message = "Login Successfull", token = login })

                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { code = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        [HttpPost("forgotpassword")]
        public ActionResult ForgotPassword(ChangePasswordVM changePasswordVM)
        {
            //return Ok(_accountRepository.ForgotPassword(emailVM));

            try
            {
                var entry = accountRepository.ForgotPassword(changePasswordVM);
                return entry switch
                {
                    0 => Ok(new { status = HttpStatusCode.OK, message = "New Password Request Successfull. Verification email has been sent." }),
                    1 => BadRequest(new { status = HttpStatusCode.BadRequest, message = "Request Failed. Email Not Found!" }),
                    2 => BadRequest(new { status = HttpStatusCode.BadRequest, message = "Request Failed. Email Found but cant send verification code!" }),
                    _ => BadRequest(new { status = HttpStatusCode.BadRequest, message = "Request Failed!" })

                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }

        [HttpPost("changepassword")]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            try
            {
                var entry = accountRepository.ChangePassword(changePasswordVM);
                return entry switch
                {
                    0 => Ok(new { status = HttpStatusCode.OK, result = changePasswordVM, message = "Password Changed Successfully" }),
                    1 => BadRequest(new { status = HttpStatusCode.BadRequest, result = changePasswordVM, message = "Request Failed. Password Doesn't Match!" }),
                    2 => BadRequest(new { status = HttpStatusCode.BadRequest, result = changePasswordVM, message = "Request Failed. Token Already Expired!" }),
                    3 => BadRequest(new { status = HttpStatusCode.BadRequest, result = changePasswordVM, message = "Request Failed. Token is Used!" }),
                    4 => BadRequest(new { status = HttpStatusCode.BadRequest, result = changePasswordVM, message = "Request Failed. Wrong Token!" }),
                    5 => BadRequest(new { status = HttpStatusCode.BadRequest, result = changePasswordVM, message = "Request Failed. Email Not Found!" }),
                    _ => BadRequest(new { status = HttpStatusCode.BadRequest, message = "Request Failed!" })

                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
            }
        }
    }
}
