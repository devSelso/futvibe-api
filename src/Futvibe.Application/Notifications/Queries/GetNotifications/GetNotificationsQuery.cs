using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(Guid UserId) : IRequest<IReadOnlyList<NotificationDto>>;
