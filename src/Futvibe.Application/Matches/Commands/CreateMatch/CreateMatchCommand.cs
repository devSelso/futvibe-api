using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Enums;
using MediatR;

namespace Futvibe.Application.Matches.Commands.CreateMatch;

public record CreateMatchCommand(
    string Title,
    string Location,
    DateOnly Date,
    TimeOnly Time,
    MatchLevel Level,
    decimal PricePerPlayer,
    int MaxPlayers,
    MatchVisibility Visibility,
    Guid HostId
) : IRequest<MatchDto>;
