using FluentValidation;
using FitNetClean.Application.Features.WorkoutGroups.Commands;

namespace FitNetClean.Application.Features.WorkoutGroups;

public class CreateWorkoutGroupRequestValidator : AbstractValidator<CreateWorkoutGroupRequest>
{
    public CreateWorkoutGroupRequestValidator()
    {
        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("WorkoutId must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");
    }
}

public class UpdateWorkoutGroupRequestValidator : AbstractValidator<UpdateWorkoutGroupRequest>
{
    public UpdateWorkoutGroupRequestValidator()
    {
        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("WorkoutId must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");
    }
}

public class AddExerciseToWorkoutGroupCommandValidator : AbstractValidator<AddExerciseToWorkoutGroupCommand>
{
    public AddExerciseToWorkoutGroupCommandValidator()
    {
        RuleFor(x => x.WorkoutGroupId)
            .GreaterThan(0)
            .WithMessage("WorkoutGroupId must be greater than 0");

        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("ExerciseId must be greater than 0");
    }
}

public class RemoveExerciseFromWorkoutGroupCommandValidator : AbstractValidator<RemoveExerciseFromWorkoutGroupCommand>
{
    public RemoveExerciseFromWorkoutGroupCommandValidator()
    {
        RuleFor(x => x.WorkoutGroupId)
            .GreaterThan(0)
            .WithMessage("WorkoutGroupId must be greater than 0");

        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("ExerciseId must be greater than 0");
    }
}
