using System;
using Client.ViewModels;
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
         return View(new NewBoard());
      }

      [HttpPost("/boards")]
      public IActionResult Index(NewBoard newBoard)
      {
         if (!ModelState.IsValid) return View(newBoard);

         return View(newBoard);
      }

      public IActionResult Detail(int id)
      {
         Console.WriteLine(id);

         return View();
      }
   }
}