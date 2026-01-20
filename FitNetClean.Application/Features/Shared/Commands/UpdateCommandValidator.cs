using FluentValidation;

namespace FitNetClean.Application.Features.Shared.Commands;

public class UpdateCommandValidator<T> : AbstractValidator<UpdateCommand<T>> where T : class
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");

        RuleFor(x => x.Entity)
            .NotNull()
            .WithMessage("Entity cannot be null");
    }
}
