using FluentValidation;

namespace Futvibe.Application.Matches.Commands.JoinMatch;

public class JoinMatchCommandValidator : AbstractValidator<JoinMatchCommand>
{
    public JoinMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
    }
}
