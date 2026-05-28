using FluentValidation;

namespace Futvibe.Application.Matches.Commands.CreateMatch;

public class CreateMatchCommandValidator : AbstractValidator<CreateMatchCommand>
{
    public CreateMatchCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Date).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Data deve ser hoje ou futura.");
        RuleFor(x => x.PricePerPlayer).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxPlayers).InclusiveBetween(2, 20);
        RuleFor(x => x.HostId).NotEmpty();
    }
}
