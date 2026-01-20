using FluentValidation;

namespace FitNetClean.Application.Features.Shared.Queries;

public class GetByIdQueryValidator<T> : AbstractValidator<GetByIdQuery<T>> where T : class
{
    public GetByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}
