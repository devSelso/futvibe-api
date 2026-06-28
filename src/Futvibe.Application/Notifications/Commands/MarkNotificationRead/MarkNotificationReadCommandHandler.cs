using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Notifications.Commands.MarkNotificationRead;

public class MarkNotificationReadCommandHandler(INotificationRepository notificationRepo)
    : IRequestHandler<MarkNotificationReadCommand>
{
    public async Task Handle(MarkNotificationReadCommand request, CancellationToken ct)
    {
        var notification = await notificationRepo.GetByIdAsync(request.NotificationId, ct)
            ?? throw new NotFoundException($"Notificação {request.NotificationId} não encontrada.");

        if (notification.UserId != request.RequestingUserId)
            throw new ForbiddenException("Acesso negado.");

        notification.MarkAsRead();

        await notificationRepo.SaveChangesAsync(ct);
    }
}
