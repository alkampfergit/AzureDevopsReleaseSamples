using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace MyWonderfulApp.Service.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/{version:apiVersion}/[controller]/[action]")]
    public class SupportController : Controller
    {
        [SwaggerResponse(typeof(String))]
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong V2");
        }
    }
}