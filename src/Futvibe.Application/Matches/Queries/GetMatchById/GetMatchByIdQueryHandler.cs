using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatchById;

public class GetMatchByIdQueryHandler(
    IMatchRepository matchRepo,
    IUserRepository userRepo,
    INotificationRepository notificationRepo)
    : IRequestHandler<GetMatchByIdQuery, MatchDto>
{
    public async Task<MatchDto> Handle(GetMatchByIdQuery request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.Id, ct)
            ?? throw new NotFoundException($"Partida {request.Id} não encontrada.");

        var previousStatus = match.Status;
        var (changed, autoPresenceIds) = match.TryAdvanceStatus();

        if (changed)
        {
            if (autoPresenceIds.Count > 0)
            {
                var users = await userRepo.GetByIdsAsync(autoPresenceIds, ct);
                foreach (var user in users)
                    user.RecordMatchPresence(present: true);
            }

            if (match.Status == MatchStatus.PendingValidation && previousStatus == MatchStatus.Scheduled)
            {
                var notification = Notification.Create(
                    match.HostId,
                    NotificationType.ValidationWindowOpened,
                    match.Id,
                    $"A janela de validação de \"{match.Title}\" está aberta. Você tem até 72h para confirmar as presenças.");
                await notificationRepo.AddAsync(notification, ct);
            }

            await matchRepo.SaveChangesAsync(ct);
        }

        return MatchMapper.ToDto(match);
    }
}
