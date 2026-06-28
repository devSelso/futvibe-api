using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.CancelMatch;

public class CancelMatchCommandHandler(
    IMatchRepository matchRepo,
    IMatchActivityRepository activityRepo,
    INotificationRepository notificationRepo) : IRequestHandler<CancelMatchCommand>
{
    public async Task Handle(CancelMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        var confirmedIds = match.Participants
            .Where(p => p.Status == ParticipantStatus.Confirmed)
            .Select(p => p.UserId)
            .ToList();

        match.Cancel(request.RequestingUserId);

        var log = MatchActivityLog.Create(request.MatchId, request.RequestingUserId, MatchActivityAction.Cancelled);
        await activityRepo.AddAsync(log, ct);

        foreach (var userId in confirmedIds)
        {
            var notification = Notification.Create(
                userId,
                NotificationType.MatchCancelled,
                match.Id,
                $"A partida \"{match.Title}\" foi cancelada pelo organizador.");
            await notificationRepo.AddAsync(notification, ct);
        }

        await matchRepo.SaveChangesAsync(ct);
    }
}
