using Microsoft.AspNetCore.Mvc;
using MyWonderfulApp.Core.DataAccess;
using MyWonderfulApp.Data.Models;
using NSwag.Annotations;
using System;

namespace MyWonderfulApp.Service.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [SwaggerTag("QueryExampleController", Description = "Version 1")]
    public class QueryExampleController : Controller
    {
        [SwaggerResponse(typeof(Product))]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetProduct(String productId)
        {
            var query = DataAccess.CreateQuery($"Select * from dbo.Products where productId = {productId}");
            var product = query.ExecuteBuildSingleEntity<Product>(Product.Builder);
            return Ok(product);
        }
    }
}
