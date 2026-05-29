using FluentValidation;

namespace Futvibe.Application.Matches.Commands.DeleteMatch;

public class DeleteMatchCommandValidator : AbstractValidator<DeleteMatchCommand>
{
    public DeleteMatchCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
    }
}
