using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("GetProduct")]
        public IActionResult GetProduct() 
        {
            return Ok("Response from Product service");
        }
    }
}
