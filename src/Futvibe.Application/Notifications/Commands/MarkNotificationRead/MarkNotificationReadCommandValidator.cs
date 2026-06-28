using FluentValidation;

namespace Futvibe.Application.Notifications.Commands.MarkNotificationRead;

public class MarkNotificationReadCommandValidator : AbstractValidator<MarkNotificationReadCommand>
{
    public MarkNotificationReadCommandValidator()
    {
        RuleFor(x => x.NotificationId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
    }
}
