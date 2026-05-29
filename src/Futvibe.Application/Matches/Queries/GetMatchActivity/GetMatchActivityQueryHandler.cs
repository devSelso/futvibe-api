using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatchActivity;

public class GetMatchActivityQueryHandler(
    IMatchRepository matchRepo,
    IMatchActivityRepository activityRepo) : IRequestHandler<GetMatchActivityQuery, IReadOnlyList<MatchActivityLogDto>>
{
    public async Task<IReadOnlyList<MatchActivityLogDto>> Handle(GetMatchActivityQuery request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        if (match.HostId != request.RequestingUserId)
            throw new ForbiddenException("Apenas o host pode ver o log de atividade.");

        var logs = await activityRepo.GetByMatchIdAsync(request.MatchId, ct);

        return logs.Select(l => new MatchActivityLogDto(
            l.Id,
            l.UserId,
            l.User?.Name ?? "Usuário",
            l.User?.Avatar,
            l.Action.ToString().ToLower(),
            l.CreatedAt
        )).ToList();
    }
}
