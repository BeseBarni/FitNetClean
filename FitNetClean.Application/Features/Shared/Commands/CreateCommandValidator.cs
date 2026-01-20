using FluentValidation;

namespace FitNetClean.Application.Features.Shared.Commands;

public class CreateCommandValidator<T> : AbstractValidator<CreateCommand<T>> where T : class
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Entity)
            .NotNull()
            .WithMessage("Entity cannot be null");
    }
}
