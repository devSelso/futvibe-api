using MediatR;

namespace Futvibe.Application.Notifications.Commands.MarkNotificationRead;

public record MarkNotificationReadCommand(Guid NotificationId, Guid RequestingUserId) : IRequest;
