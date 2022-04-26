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
    public class CheckListItemsController : BaseController<CheckListItem, CheckListItemRepository, string>
    {
        public CheckListItemsController(CheckListItemRepository CheckListItemRepository, MyContext myContext) : base(CheckListItemRepository)
        {
           
        }
    }
}
