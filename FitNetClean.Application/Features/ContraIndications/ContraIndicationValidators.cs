using FluentValidation;
using FitNetClean.Application.Features.ContraIndications.Queries;

namespace FitNetClean.Application.Features.ContraIndications;

public class CreateContraIndicationRequestValidator : AbstractValidator<CreateContraIndicationRequest>
{
    public CreateContraIndicationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");
    }
}

public class UpdateContraIndicationRequestValidator : AbstractValidator<UpdateContraIndicationRequest>
{
    public UpdateContraIndicationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");
    }
}

public class GetContraIndicationRelatedQueryValidator : AbstractValidator<GetContraIndicationRelatedQuery>
{
    public GetContraIndicationRelatedQueryValidator()
    {
        RuleFor(x => x.ContraIndicationId)
            .GreaterThan(0)
            .WithMessage("ContraIndicationId must be greater than 0");
    }
}
