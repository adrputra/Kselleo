using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kselleo.Controllers
{
   public class BoardsController : Controller
   {
      private readonly ILogger<BoardsController> _logger;

      public BoardsController(ILogger<BoardsController> logger)
      {
         _logger = logger;
      }

      [HttpGet("/boards")]
      public IActionResult Index()
      {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            PageBoardVM pageBoardVM = new PageBoardVM();
            pageBoardVM.DecodeJwtVM = GetDecodeJwt();

            return View(pageBoardVM);
        }

      [HttpPost("/boards")]
      public IActionResult Index(NewBoard newBoard)
      {
         if (!ModelState.IsValid) return View(newBoard);

         return View(newBoard);
      }

      public IActionResult Detail(int id)
      {
         var token = HttpContext.Session.GetString("JWToken");
         if (token == null) return RedirectToAction("Login", "Auth");

         ViewBag.BoardId = id;
         return View();
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

      public IActionResult Member()
      {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null) return RedirectToAction("Login", "Auth");

            return View();
      }
   }
}