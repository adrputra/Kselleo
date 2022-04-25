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
    public class UsersController : BaseController<User, UserRepository, string>
    {
        public UsersController(UserRepository UserRepository, MyContext myContext) : base(UserRepository)
        {
           
        }
    }
}
