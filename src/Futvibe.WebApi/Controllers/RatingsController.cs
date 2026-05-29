using Futvibe.Application.Ratings.Commands.SubmitRating;
using Futvibe.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Futvibe.WebApi.Controllers;

[ApiController]
[Route("api/ratings")]
[Authorize]
public class RatingsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitRatingBody body, CancellationToken ct)
    {
        var command = new SubmitRatingCommand(User.GetUserId(), body.RatedId, body.MatchId, body.Score);
        await mediator.Send(command, ct);
        return StatusCode(201);
    }
}

public record SubmitRatingBody(Guid RatedId, Guid MatchId, int Score);
