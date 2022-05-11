using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
   public class AdminController : Controller
   {
      private readonly ILogger<AdminController> _logger;

      public AdminController(ILogger<AdminController> logger)
      {
         _logger = logger;
      }

      
      public IActionResult Index()
      {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            PageAdminVM pageAdminVM = new PageAdminVM();
            pageAdminVM.DecodeJwtVM = GetDecodeJwt();
            return View();
      }

      
      public IActionResult Dashboard()
      {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            PageAdminVM pageAdminVM = new PageAdminVM();
            pageAdminVM.DecodeJwtVM = GetDecodeJwt();

            return View(pageAdminVM);
        }

      public IActionResult Users()
      {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            PageAdminVM pageAdminVM = new PageAdminVM();
            pageAdminVM.DecodeJwtVM = GetDecodeJwt();
            return View(pageAdminVM);
        }

        public IActionResult Profile()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            PageAdminVM pageAdminVM = new PageAdminVM();
            pageAdminVM.DecodeJwtVM = GetDecodeJwt();
            return View(pageAdminVM);
        }

        public DecodeJwtVM GetDecodeJwt()
        {
            var token = HttpContext.Session.GetString("JWToken");

            var handler = new JwtSecurityTokenHandler();
            var decode = handler.ReadJwtToken(token);

            var id = decode.Claims.First(claim => claim.Type == "Id").Value;
            var role = decode.Claims.First(claim => claim.Type == "Roles").Value;
            var fullName = decode.Claims.First(claim => claim.Type == "Fullname").Value;
            var image = decode.Claims.First(claim => claim.Type == "Image").Value;
            var email = decode.Claims.First(claim => claim.Type == "Email").Value;

            var decodeJWT = new DecodeJwtVM
            {
                Id = id,
                FullName = fullName,
                Image = image,
                Email = email,
                Roles = role
            };

            return decodeJWT;
        }


    }
}
