using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
         return View();
      }
      public IActionResult Dashboard()
      {
         ViewBag.isDashboard = true;
         ViewBag.isUsers = false;
         return View();
      }

      public IActionResult Users()
      {
         ViewBag.isDashboard = false;
         ViewBag.isUsers = true;
         return View();
      }
   }
}
