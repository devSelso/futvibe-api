using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatchActivity;

public record GetMatchActivityQuery(Guid MatchId, Guid RequestingUserId) : IRequest<IReadOnlyList<MatchActivityLogDto>>;
