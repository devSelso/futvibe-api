using FluentValidation;

namespace Futvibe.Application.Matches.Commands.UpdateParticipantStatus;

public class UpdateParticipantStatusCommandValidator : AbstractValidator<UpdateParticipantStatusCommand>
{
    public UpdateParticipantStatusCommandValidator()
    {
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
        RuleFor(x => x.RequestingUserId).NotEmpty();
        RuleFor(x => x.NewStatus)
            .IsInEnum()
            .WithMessage("Status inválido.");
    }
}
