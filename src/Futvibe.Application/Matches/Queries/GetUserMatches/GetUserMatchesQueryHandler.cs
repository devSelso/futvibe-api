using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetUserMatches;

public class GetUserMatchesQueryHandler(IMatchRepository matchRepo)
    : IRequestHandler<GetUserMatchesQuery, IReadOnlyList<MatchDto>>
{
    public async Task<IReadOnlyList<MatchDto>> Handle(GetUserMatchesQuery request, CancellationToken ct)
    {
        var matches = await matchRepo.GetByUserIdAsync(request.UserId, ct);
        return matches.Select(MatchMapper.ToDto).ToList();
    }
}
