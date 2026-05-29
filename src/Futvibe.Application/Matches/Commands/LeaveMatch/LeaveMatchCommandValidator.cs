using FluentValidation;

namespace Futvibe.Application.Matches.Commands.LeaveMatch;

public class LeaveMatchCommandValidator : AbstractValidator<LeaveMatchCommand>
{
    public LeaveMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
