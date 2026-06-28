using FluentValidation;

namespace Futvibe.Application.Notifications.Queries.GetNotifications;

public class GetNotificationsQueryValidator : AbstractValidator<GetNotificationsQuery>
{
    public GetNotificationsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
