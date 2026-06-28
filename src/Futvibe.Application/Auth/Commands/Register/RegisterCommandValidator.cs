using FluentValidation;

namespace Futvibe.Application.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private static readonly string[] ValidLevels = ["beginner", "intermediate", "advanced"];

    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Level)
            .NotEmpty()
            .Must(l => ValidLevels.Contains(l?.ToLower()))
            .WithMessage("Nível deve ser beginner, intermediate ou advanced.");
        RuleFor(x => x.City).NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres.");
    }
}
