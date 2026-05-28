using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetUserMatches;

public record GetUserMatchesQuery(Guid UserId) : IRequest<IReadOnlyList<MatchDto>>;
