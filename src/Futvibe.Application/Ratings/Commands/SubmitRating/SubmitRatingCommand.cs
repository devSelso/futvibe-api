using MediatR;

namespace Futvibe.Application.Ratings.Commands.SubmitRating;

public record SubmitRatingCommand(
    Guid RaterId,
    Guid RatedId,
    Guid MatchId,
    int Score
) : IRequest;
