using Futvibe.Application.Auth.Commands.Login;
using Futvibe.Application.Auth.Commands.Logout;
using Futvibe.Application.Auth.Commands.Refresh;
using Futvibe.Application.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator, IWebHostEnvironment env) : ControllerBase
{
    private const string RefreshTokenCookie = "refreshToken";

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        SetRefreshTokenCookie(result.RawRefreshToken);
        return Ok(new { token = result.Token, user = result.User });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        SetRefreshTokenCookie(result.RawRefreshToken);
        return StatusCode(201, new { token = result.Token, user = result.User });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var rawToken = Request.Cookies[RefreshTokenCookie];
        if (string.IsNullOrEmpty(rawToken))
            return Unauthorized();

        var result = await mediator.Send(new RefreshTokenCommand(rawToken), ct);
        SetRefreshTokenCookie(result.RawRefreshToken);
        return Ok(new { token = result.AccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var rawToken = Request.Cookies[RefreshTokenCookie];
        if (!string.IsNullOrEmpty(rawToken))
            await mediator.Send(new LogoutCommand(rawToken), ct);

        Response.Cookies.Delete(RefreshTokenCookie, new CookieOptions { Path = "/api/auth" });
        return NoContent();
    }

    private void SetRefreshTokenCookie(string rawToken)
    {
        Response.Cookies.Append(RefreshTokenCookie, rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !env.IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            Path = "/api/auth"
        });
    }
}
