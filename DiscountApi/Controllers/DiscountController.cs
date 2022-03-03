using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DiscountController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var userName = User?.Identity?.Name;
        var userIdClaim = User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier);
        return Ok($"UserName:{userName}, UserId:{userIdClaim?.Value}");
    }
}
