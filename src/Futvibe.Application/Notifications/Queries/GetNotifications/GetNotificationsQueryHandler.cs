using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryHandler(INotificationRepository notificationRepo)
    : IRequestHandler<GetNotificationsQuery, IReadOnlyList<NotificationDto>>
{
    public async Task<IReadOnlyList<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken ct)
    {
        var notifications = await notificationRepo.GetUnreadByUserIdAsync(request.UserId, ct);

        return notifications
            .Select(n => new NotificationDto(n.Id, n.Type, n.MatchId, n.Message, n.IsRead, n.CreatedAt))
            .ToList();
    }
}
