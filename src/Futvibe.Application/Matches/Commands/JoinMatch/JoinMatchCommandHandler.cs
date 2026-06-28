using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.JoinMatch;

public class JoinMatchCommandHandler(
    IMatchRepository matchRepo,
    IUserRepository userRepo,
    IMatchActivityRepository activityRepo,
    INotificationRepository notificationRepo) : IRequestHandler<JoinMatchCommand>
{
    public async Task Handle(JoinMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        var status = match.DetermineStatusForNewJoiner();
        match.AddParticipant(request.RequestingUserId, status);

        var log = MatchActivityLog.Create(request.MatchId, request.RequestingUserId, MatchActivityAction.Requested);
        await activityRepo.AddAsync(log, ct);

        if (status is ParticipantStatus.Pending or ParticipantStatus.Waitlist)
        {
            var requester = await userRepo.GetByIdAsync(request.RequestingUserId, ct);
            var requesterName = requester?.Name ?? "Alguém";
            var label = status == ParticipantStatus.Waitlist ? "entrou na lista de espera" : "solicitou vaga";
            var message = $"{requesterName} {label} em \"{match.Title}\".";
            var notification = Notification.Create(match.HostId, NotificationType.JoinRequested, match.Id, message);
            await notificationRepo.AddAsync(notification, ct);
        }

        await matchRepo.SaveChangesAsync(ct);
    }
}
