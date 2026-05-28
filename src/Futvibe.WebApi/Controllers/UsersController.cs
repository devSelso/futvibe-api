using Futvibe.Application.Users.Commands.UpdateProfile;
using Futvibe.Application.Users.Queries.GetCurrentUser;
using Futvibe.Application.Users.Queries.GetUserById;
using Futvibe.Domain.Enums;
using Futvibe.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe(CancellationToken ct)
        => Ok(await mediator.Send(new GetCurrentUserQuery(User.GetUserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new GetUserByIdQuery(id), ct));

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileBody body, CancellationToken ct)
    {
        var command = new UpdateProfileCommand(User.GetUserId(), body.Name, body.Bio, body.Level);
        return Ok(await mediator.Send(command, ct));
    }
}

public record UpdateProfileBody(string Name, string? Bio, MatchLevel Level);
