using FluentValidation;
using FitNetClean.Application.Features.Workouts.Commands;
using FitNetClean.Application.Features.Workouts.Queries;

namespace FitNetClean.Application.Features.Workouts;

public class CreateWorkoutRequestValidator : AbstractValidator<CreateWorkoutRequest>
{
    public CreateWorkoutRequestValidator()
    {
        RuleFor(x => x.CodeName)
            .NotEmpty()
            .WithMessage("CodeName is required")
            .MaximumLength(50)
            .WithMessage("CodeName must not exceed 50 characters");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters")
            .When(x => x.Description != null);

        RuleFor(x => x.WarmupDurationMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Warmup duration must be 0 or greater");

        RuleFor(x => x.MainWorkoutDurationMinutes)
            .GreaterThan(0)
            .WithMessage("Main workout duration is required and must be greater than 0");
    }
}

public class UpdateWorkoutRequestValidator : AbstractValidator<UpdateWorkoutRequest>
{
    public UpdateWorkoutRequestValidator()
    {
        RuleFor(x => x.CodeName)
            .NotEmpty()
            .WithMessage("CodeName is required")
            .MaximumLength(50)
            .WithMessage("CodeName must not exceed 50 characters");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters")
            .When(x => x.Description != null);

        RuleFor(x => x.WarmupDurationMinutes)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Warmup duration must be 0 or greater");

        RuleFor(x => x.MainWorkoutDurationMinutes)
            .GreaterThan(0)
            .WithMessage("Main workout duration is required and must be greater than 0");
    }
}

public class GetWorkoutDetailQueryValidator : AbstractValidator<GetWorkoutDetailQuery>
{
    public GetWorkoutDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}

public class AddWorkoutGroupToWorkoutCommandValidator : AbstractValidator<AddWorkoutGroupToWorkoutCommand>
{
    public AddWorkoutGroupToWorkoutCommandValidator()
    {
        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("WorkoutId must be greater than 0");

        RuleFor(x => x.WorkoutGroupId)
            .GreaterThan(0)
            .WithMessage("WorkoutGroupId must be greater than 0");
    }
}
