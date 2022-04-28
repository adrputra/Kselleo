using Client.Base;
using Client.Models;
using Client.Repositories.Data;
using Client.ViewModels;
using Kselleo.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controllers
{
   public class AuthController : BaseController<LoginVM, AuthRepository, int>
   {
      private readonly ILogger<AuthController> _logger;
      private readonly AuthRepository repository;

      public AuthController(ILogger<AuthController> logger, AuthRepository repository) : base(repository)
      {
         _logger = logger;
         this.repository = repository;
      }


      public IActionResult Index()
      {
         return View();
      }

      public IActionResult Login()
      {
         var token = HttpContext.Session.GetString("JWToken");
         if (token != null) return RedirectToAction("Index", "Boards");


         ViewBag.IsError = false;

         return View(new LoginVM());
      }

      [HttpPost]
      public async Task<IActionResult> Login(LoginVM loginVM)
      {
         Console.WriteLine($"email => {loginVM.Email}, password => {loginVM.Password}");

         var jwtToken = await repository.Auth(loginVM);
         Console.WriteLine($"jwt token => {jwtToken.token}");
         var token = jwtToken.token;

         if (token == null)
         {
            ViewBag.IsError = true;
            ViewBag.Message = jwtToken.message;
            return View("login");
         }

         HttpContext.Session.SetString("JWToken", token);

         var handler = new JwtSecurityTokenHandler();

         var decode = handler.ReadJwtToken(token);

         var role = decode.Claims.First(claim => claim.Type == "Roles").Value;
         var fullName = decode.Claims.First(claim => claim.Type == "Fullname").Value;
         var image = decode.Claims.First(claim => claim.Type == "Image").Value;
         var email = decode.Claims.First(claim => claim.Type == "Email").Value;

         var decodeJWT = new DecodeJwtVM
         {
            FullName = fullName,
            Image = image,
            Email = email,
            Roles = role
         };

         if (role == "Admin")
         {
            return RedirectToAction("dashboard", "admin");
         }
         else
         {
            return RedirectToAction("index", "boards");
         }

      }

      public IActionResult Register()
      {
         var token = HttpContext.Session.GetString("JWToken");
         if (token != null) return RedirectToAction("Index", "Boards");

         return View(new RegisterVM());
      }

      [HttpPost]
      public IActionResult Register(RegisterVM registerVM)
      {
         if (!ModelState.IsValid) return View(registerVM);

         return View(registerVM);
      }
      public IActionResult ForgotPassword()
      {
         return View(new ForgetPasswordVM());
      }

      public IActionResult ChangePassword()
      {
         return View(new ChangePasswordVM());
      }

      public IActionResult Logout()
      {
         HttpContext.Session.Remove("JWToken");
         return RedirectToAction("Login", "Auth");
      }
   }
}
