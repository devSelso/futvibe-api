using Futvibe.Application.Matches.Commands.CreateMatch;
using Futvibe.Application.Matches.Commands.JoinMatch;
using Futvibe.Application.Matches.Commands.UpdateParticipantStatus;
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

    [HttpPost("{id:guid}/join")]
    [Authorize]
    public async Task<IActionResult> Join(Guid id, CancellationToken ct)
    {
        await mediator.Send(new JoinMatchCommand(id, User.GetUserId()), ct);
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
