using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Items = new[]
            {
                new { Id = 1, Items = new[] { new { Id = 1, Price = 159.99 }, new { Id = 2, Price = 299.99 } }, CreatedOn = DateTime.Now },
                new { Id = 2, Items = new[] { new { Id = 3, Price = 189.00 } }, CreatedOn = DateTime.Now },
            }
        });
    }
}