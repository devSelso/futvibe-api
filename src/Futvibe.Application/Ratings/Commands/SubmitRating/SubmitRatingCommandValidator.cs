using FluentValidation;

namespace Futvibe.Application.Ratings.Commands.SubmitRating;

public class SubmitRatingCommandValidator : AbstractValidator<SubmitRatingCommand>
{
    public SubmitRatingCommandValidator()
    {
        RuleFor(x => x.RaterId).NotEmpty();
        RuleFor(x => x.RatedId).NotEmpty();
        RuleFor(x => x.MatchId).NotEmpty();
        RuleFor(x => x.Score).InclusiveBetween(0, 5).WithMessage("Nota deve ser entre 0 e 5.");
    }
}
