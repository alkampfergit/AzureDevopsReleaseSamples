using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace MyWonderfulApp.Service.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [SwaggerTag("SupportController", Description = "Version 1")]
    public class SupportController : Controller
    {
        [SwaggerResponse(typeof(String))]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}