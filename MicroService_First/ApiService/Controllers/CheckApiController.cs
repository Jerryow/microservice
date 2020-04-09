using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckApiController : ControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Check()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}