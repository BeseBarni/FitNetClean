using FluentValidation;

namespace FitNetClean.Application.Features.Shared.Commands;

public class DeleteCommandValidator<T> : AbstractValidator<DeleteCommand<T>> where T : class
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}
