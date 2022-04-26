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
   }
}