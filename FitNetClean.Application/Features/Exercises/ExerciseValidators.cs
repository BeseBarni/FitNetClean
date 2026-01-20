using FluentValidation;
using FitNetClean.Application.Features.Exercises.Commands;
using FitNetClean.Application.Features.Exercises.Queries;

namespace FitNetClean.Application.Features.Exercises;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Repetition)
            .NotNull()
            .WithMessage("Repetition is required");

        RuleFor(x => x.Repetition.Quantity)
            .GreaterThan(0)
            .WithMessage("Repetition quantity must be greater than 0")
            .When(x => x.Repetition != null);

        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("WorkoutId must be greater than 0");

        RuleFor(x => x.WorkoutGroupId)
            .GreaterThan(0)
            .WithMessage("WorkoutGroupId must be greater than 0")
            .When(x => x.WorkoutGroupId.HasValue);

        RuleFor(x => x.EquipmentId)
            .GreaterThan(0)
            .WithMessage("EquipmentId must be greater than 0")
            .When(x => x.EquipmentId.HasValue);
    }
}

public class UpdateExerciseRequestValidator : AbstractValidator<UpdateExerciseRequest>
{
    public UpdateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Repetition)
            .NotNull()
            .WithMessage("Repetition is required");

        RuleFor(x => x.Repetition.Quantity)
            .GreaterThan(0)
            .WithMessage("Repetition quantity must be greater than 0")
            .When(x => x.Repetition != null);

        RuleFor(x => x.WorkoutId)
            .GreaterThan(0)
            .WithMessage("WorkoutId must be greater than 0");

        RuleFor(x => x.WorkoutGroupId)
            .GreaterThan(0)
            .WithMessage("WorkoutGroupId must be greater than 0")
            .When(x => x.WorkoutGroupId.HasValue);

        RuleFor(x => x.EquipmentId)
            .GreaterThan(0)
            .WithMessage("EquipmentId must be greater than 0")
            .When(x => x.EquipmentId.HasValue);
    }
}

public class AddContraIndicationToExerciseCommandValidator : AbstractValidator<AddContraIndicationToExerciseCommand>
{
    public AddContraIndicationToExerciseCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("ExerciseId must be greater than 0");

        RuleFor(x => x.ContraIndicationId)
            .GreaterThan(0)
            .WithMessage("ContraIndicationId must be greater than 0");
    }
}

public class RemoveContraIndicationFromExerciseCommandValidator : AbstractValidator<RemoveContraIndicationFromExerciseCommand>
{
    public RemoveContraIndicationFromExerciseCommandValidator()
    {
        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("ExerciseId must be greater than 0");

        RuleFor(x => x.ContraIndicationId)
            .GreaterThan(0)
            .WithMessage("ContraIndicationId must be greater than 0");
    }
}

public class GetExerciseWorkoutProgramsQueryValidator : AbstractValidator<GetExerciseWorkoutProgramsQuery>
{
    public GetExerciseWorkoutProgramsQueryValidator()
    {
        RuleFor(x => x.ExerciseId)
            .GreaterThan(0)
            .WithMessage("ExerciseId must be greater than 0");
    }
}
