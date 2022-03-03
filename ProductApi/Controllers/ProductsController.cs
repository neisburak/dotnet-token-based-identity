using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Items = new[]
            {
                new { Id = 1, Name = "Phone", Price = 159.99, CreatedOn = DateTime.Now },
                new { Id = 2, Name = "Laptop", Price = 299.99, CreatedOn = DateTime.Now },
                new { Id = 3, Name = "Monitor", Price = 189.00, CreatedOn = DateTime.Now },
            }
        });
    }
}
