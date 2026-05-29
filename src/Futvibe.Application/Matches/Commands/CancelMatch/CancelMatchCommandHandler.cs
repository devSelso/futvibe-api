using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.CancelMatch;

public class CancelMatchCommandHandler(
    IMatchRepository matchRepo,
    IMatchActivityRepository activityRepo) : IRequestHandler<CancelMatchCommand>
{
    public async Task Handle(CancelMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.Cancel(request.RequestingUserId);

        var log = MatchActivityLog.Create(request.MatchId, request.RequestingUserId, MatchActivityAction.Cancelled);
        await activityRepo.AddAsync(log, ct);

        await matchRepo.SaveChangesAsync(ct);
    }
}
