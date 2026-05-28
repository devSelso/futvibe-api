using Futvibe.Application.Auth.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        => Ok(await mediator.Send(command, ct));
}
