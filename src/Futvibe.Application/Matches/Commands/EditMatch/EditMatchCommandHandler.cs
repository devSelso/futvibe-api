using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.EditMatch;

public class EditMatchCommandHandler(
    IMatchRepository matchRepo,
    INotificationRepository notificationRepo)
    : IRequestHandler<EditMatchCommand>
{
    public async Task Handle(EditMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.Edit(
            request.RequestingUserId,
            request.Title, request.Location, request.City,
            request.Date, request.Time,
            request.Level, request.PricePerPlayer,
            request.MaxPlayers, request.Visibility);

        var confirmedIds = match.Participants
            .Where(p => p.Status == ParticipantStatus.Confirmed)
            .Select(p => p.UserId);

        foreach (var userId in confirmedIds)
        {
            var notification = Notification.Create(
                userId,
                NotificationType.MatchUpdated,
                match.Id,
                $"A partida \"{match.Title}\" foi atualizada pelo organizador.");
            await notificationRepo.AddAsync(notification, ct);
        }

        await matchRepo.SaveChangesAsync(ct);
    }
}
