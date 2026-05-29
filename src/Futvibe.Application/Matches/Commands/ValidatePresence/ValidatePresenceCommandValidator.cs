using FluentValidation;

namespace Futvibe.Application.Matches.Commands.ValidatePresence;

public class ValidatePresenceCommandValidator : AbstractValidator<ValidatePresenceCommand>
{
    public ValidatePresenceCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
        RuleFor(x => x.Validations).NotEmpty().WithMessage("Informe ao menos um participante.");
        RuleForEach(x => x.Validations).ChildRules(v =>
            v.RuleFor(p => p.UserId).NotEmpty());
    }
}
