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
        public CardsController(CardRepository CardRepository, MyContext myContext) : base(CardRepository)
        {
           
        }
    }
}
