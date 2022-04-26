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
    public class CheckListItemAssignsController : BaseController<CheckListItemAssign, CheckListItemAssignRepository, string>
    {
        public CheckListItemAssignsController(CheckListItemAssignRepository CheckListItemAssignRepository, MyContext myContext) : base(CheckListItemAssignRepository)
        {
           
        }
    }
}
