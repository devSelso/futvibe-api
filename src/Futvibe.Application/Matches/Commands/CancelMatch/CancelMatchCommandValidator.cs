using FluentValidation;

namespace Futvibe.Application.Matches.Commands.CancelMatch;

public class CancelMatchCommandValidator : AbstractValidator<CancelMatchCommand>
{
    public CancelMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
    }
}
