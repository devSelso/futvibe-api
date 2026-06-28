using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatches;

public record GetMatchesQuery(
    string? Location = null,
    decimal? MaxPrice = null,
    int Page = 1,
    int Limit = 10
) : IRequest<IReadOnlyList<MatchDto>>;
