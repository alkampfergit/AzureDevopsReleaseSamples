using Microsoft.AspNetCore.Mvc;
using MyWonderfulApp.Service.Models.V1;
using NSwag.Annotations;
using System;

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

        [SwaggerResponse(typeof(DoStuffResult))]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult DoStuff(Int32 number)
        {
            String userName = "Guest";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.User.Identity.Name;
            }
            return Ok(new DoStuffResult(number * 42, $"Hello: {userName}"));
        }
    }
}