using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DiscountsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Items = new[]
            {
                new { Id = 1, ProductId = 3, Discount = 12.25 },
                new { Id = 2, ProductId = 2, Discount = 20.50 },
                new { Id = 3, ProductId = 1, Discount = 15.0 },
            }
        });
    }
}
