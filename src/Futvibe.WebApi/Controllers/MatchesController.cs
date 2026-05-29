using Futvibe.Application.Matches.Commands.CreateMatch;
using Futvibe.Application.Matches.Commands.CancelMatch;
using Futvibe.Application.Matches.Commands.DeleteMatch;
using Futvibe.Application.Matches.Commands.EditMatch;
using Futvibe.Application.Matches.Commands.JoinMatch;
using Futvibe.Application.Matches.Commands.LeaveMatch;
using Futvibe.Application.Matches.Commands.UpdateParticipantStatus;
using Futvibe.Application.Matches.Commands.ValidatePresence;
using Futvibe.Application.Matches.Queries.GetMatchActivity;
using Futvibe.Application.Matches.Queries.GetMatchById;
using Futvibe.Application.Matches.Queries.GetMatches;
using Futvibe.Application.Matches.Queries.GetUserMatches;
using Futvibe.Domain.Enums;
using Futvibe.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetMatchesQuery query, CancellationToken ct)
        => Ok(await mediator.Send(query, ct));

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyMatches(CancellationToken ct)
        => Ok(await mediator.Send(new GetUserMatchesQuery(User.GetUserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new GetMatchByIdQuery(id), ct));

    [HttpGet("{id:guid}/activity")]
    [Authorize]
    public async Task<IActionResult> GetActivity(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new GetMatchActivityQuery(id, User.GetUserId()), ct));

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateMatchBody body, CancellationToken ct)
    {
        var command = new CreateMatchCommand(
            body.Title, body.Location, body.Date, body.Time,
            body.Level, body.PricePerPlayer, body.MaxPlayers,
            body.Visibility, User.GetUserId());

        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        await mediator.Send(new CancelMatchCommand(id, User.GetUserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteMatchCommand(id, User.GetUserId()), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Edit(Guid id, [FromBody] EditMatchBody body, CancellationToken ct)
    {
        var command = new EditMatchCommand(
            id, User.GetUserId(),
            body.Title, body.Location, body.Date, body.Time,
            body.Level, body.PricePerPlayer, body.MaxPlayers, body.Visibility);

        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/join")]
    [Authorize]
    public async Task<IActionResult> Join(Guid id, CancellationToken ct)
    {
        await mediator.Send(new JoinMatchCommand(id, User.GetUserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}/leave")]
    [Authorize]
    public async Task<IActionResult> Leave(Guid id, CancellationToken ct)
    {
        await mediator.Send(new LeaveMatchCommand(id, User.GetUserId()), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/validate-presence")]
    [Authorize]
    public async Task<IActionResult> ValidatePresence(Guid id, [FromBody] ValidatePresenceBody body, CancellationToken ct)
    {
        var command = new ValidatePresenceCommand(
            id,
            User.GetUserId(),
            body.Validations.Select(v => new ParticipantPresence(v.UserId, v.Present)).ToList());

        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{matchId:guid}/participants/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateParticipant(
        Guid matchId, Guid userId,
        [FromBody] UpdateParticipantBody body,
        CancellationToken ct)
    {
        var command = new UpdateParticipantStatusCommand(matchId, userId, User.GetUserId(), body.Status);
        await mediator.Send(command, ct);
        return NoContent();
    }
}

public record EditMatchBody(
    string Title,
    string Location,
    DateOnly Date,
    TimeOnly Time,
    MatchLevel Level,
    decimal PricePerPlayer,
    int MaxPlayers,
    MatchVisibility Visibility);

public record CreateMatchBody(
    string Title,
    string Location,
    DateOnly Date,
    TimeOnly Time,
    MatchLevel Level,
    decimal PricePerPlayer,
    int MaxPlayers,
    MatchVisibility Visibility);

public record UpdateParticipantBody(ParticipantStatus Status);
public record ValidatePresenceBody(IReadOnlyList<ParticipantPresenceItem> Validations);
public record ParticipantPresenceItem(Guid UserId, bool Present);
