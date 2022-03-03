using Core.Interfaces;
using Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : BaseController
{
    private readonly IAuthenticationService _authService;

    public AuthController(IAuthenticationService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateToken(SignIn signIn)
    {
        var result = await _authService.CreateTokenAsync(signIn);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public IActionResult CreateTokenByClient(ClientSignIn signIn)
    {
        var result = _authService.CreateToken(signIn);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(refreshToken);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);

        return ActionResultInstance(result);
    }
}
