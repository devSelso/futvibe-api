using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.LeaveMatch;

public class LeaveMatchCommandHandler(
    IMatchRepository matchRepo,
    IMatchActivityRepository activityRepo) : IRequestHandler<LeaveMatchCommand>
{
    public async Task Handle(LeaveMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.RemoveParticipant(request.UserId);

        var log = MatchActivityLog.Create(request.MatchId, request.UserId, MatchActivityAction.Left);
        await activityRepo.AddAsync(log, ct);

        await matchRepo.SaveChangesAsync(ct);
    }
}
