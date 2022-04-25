using Client.Models;
using Client.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
   public class AuthController : Controller
   {
      private readonly ILogger<AuthController> _logger;

      public AuthController(ILogger<AuthController> logger)
      {
         _logger = logger;
      }


      public IActionResult Index()
      {
         return View();
      }

      public IActionResult Login()
      {
         return View(new LoginVM());
      }

      [HttpPost]
      public IActionResult Login(LoginVM loginVM)
      {
         if (!ModelState.IsValid) return View(loginVM);

         return View(loginVM);
      }
   }
}
