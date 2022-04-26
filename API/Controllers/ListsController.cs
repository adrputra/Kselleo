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
    public class ListsController : BaseController<List, ListRepository, int>
    {
        public ListsController(ListRepository ListRepository, MyContext myContext) : base(ListRepository)
        {
           
        }
    }
}
