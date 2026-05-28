using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Enums;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatches;

public record GetMatchesQuery(
    MatchLevel? Level = null,
    bool? Paid = null,
    int Page = 1,
    int Limit = 10
) : IRequest<IReadOnlyList<MatchDto>>;
