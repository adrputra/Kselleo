using System;
using System.Net;
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
   public class CardsController : BaseController<Card, CardRepository, string>
   {
      CardRepository cardRepository;
      public CardsController(CardRepository CardRepository, MyContext myContext) : base(CardRepository)
      {
         this.cardRepository = CardRepository;
      }

      [HttpGet("detail/{Id}")]
      public ActionResult GetCard(int Id)
      {
         try
         {
            var card = cardRepository.DetailCard(Id);
            return StatusCode(200, new { code = HttpStatusCode.OK, message = $"Get Card {Id} Successfully!", data = card });
         }
         catch (Exception ex)
         {
            return StatusCode(500, new { status = HttpStatusCode.InternalServerError, message = ex.Message });
         }
      }
   }
}
