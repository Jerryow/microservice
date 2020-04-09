using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IService;

namespace ApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserApiController(IUserService userService)
        {
            this._userService = userService;
        }


        [HttpGet]
        public IService.Models.User Get()
        {
            return _userService.GetID(1);
        }
    }
}