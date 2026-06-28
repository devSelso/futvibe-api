using Futvibe.Domain.Enums;
using MediatR;

namespace Futvibe.Application.Matches.Commands.EditMatch;

public record EditMatchCommand(
    Guid MatchId,
    Guid RequestingUserId,
    string Title,
    string Location,
    string City,
    DateOnly Date,
    TimeOnly Time,
    MatchLevel Level,
    decimal PricePerPlayer,
    int MaxPlayers,
    MatchVisibility Visibility
) : IRequest;
