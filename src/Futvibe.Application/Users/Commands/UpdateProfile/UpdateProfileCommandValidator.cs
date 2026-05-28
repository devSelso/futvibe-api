using FluentValidation;

namespace Futvibe.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Bio).MaximumLength(200).When(x => x.Bio is not null);
        RuleFor(x => x.Level).IsInEnum();
    }
}
