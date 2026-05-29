using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatches;

public class GetMatchesQueryHandler(IMatchRepository matchRepo)
    : IRequestHandler<GetMatchesQuery, IReadOnlyList<MatchDto>>
{
    public async Task<IReadOnlyList<MatchDto>> Handle(GetMatchesQuery request, CancellationToken ct)
    {
        var matches = await matchRepo.GetAllAsync(request.Location, request.Page, request.Limit, ct);
        return matches.Select(MatchMapper.ToDto).ToList();
    }
}
