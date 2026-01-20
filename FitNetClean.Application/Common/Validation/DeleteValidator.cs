using FitNetClean.Application.Common;
using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Common.Validation;

public class DeleteValidator(IFitnetContext context) : IDeleteValidator
{
    public async Task<(bool CanDelete, List<string> Dependencies)> ValidateDeleteAsync<T>(long id, CancellationToken ct) where T : class
    {
        var dependencies = new List<string>();

        // Check based on entity type
        if (typeof(T) == typeof(Category))
        {
            dependencies = await ValidateCategoryDeleteAsync(id, ct);
        }
        else if (typeof(T) == typeof(ContraIndication))
        {
            dependencies = await ValidateContraIndicationDeleteAsync(id, ct);
        }
        else if (typeof(T) == typeof(Equipment))
        {
            dependencies = await ValidateEquipmentDeleteAsync(id, ct);
        }
        else if (typeof(T) == typeof(Exercise))
        {
            // Exercises typically don't have dependencies that prevent deletion
            dependencies = new List<string>();
        }
        else if (typeof(T) == typeof(Workout))
        {
            dependencies = await ValidateWorkoutDeleteAsync(id, ct);
        }
        else if (typeof(T) == typeof(WorkoutGroup))
        {
            dependencies = await ValidateWorkoutGroupDeleteAsync(id, ct);
        }

        return (dependencies.Count == 0, dependencies);
    }

    private async Task<List<string>> ValidateCategoryDeleteAsync(long id, CancellationToken ct)
    {
        var dependencies = new List<string>();

        // Check for active equipment in this category
        var activeEquipmentCount = await context.Equipment
            .Where(e => e.CategoryId == id && !e.IsDeleted)
            .CountAsync(ct);

        if (activeEquipmentCount > 0)
        {
            dependencies.Add($"{activeEquipmentCount} active equipment item(s)");
        }

        return dependencies;
    }

    private async Task<List<string>> ValidateContraIndicationDeleteAsync(long id, CancellationToken ct)
    {
        var dependencies = new List<string>();

        var contraIndication = await context.ContraIndication
            .Include(ci => ci.ExerciseList)
            .Include(ci => ci.EquipmentList)
            .FirstOrDefaultAsync(ci => ci.Id == id, ct);

        if (contraIndication == null)
            return dependencies;

        // Check for active exercises with this contraindication
        var activeExercisesCount = contraIndication.ExerciseList
            .Count(e => !e.IsDeleted);

        if (activeExercisesCount > 0)
        {
            dependencies.Add($"{activeExercisesCount} active exercise(s)");
        }

        // Check for active equipment with this contraindication
        var activeEquipmentCount = contraIndication.EquipmentList
            .Count(e => !e.IsDeleted);

        if (activeEquipmentCount > 0)
        {
            dependencies.Add($"{activeEquipmentCount} active equipment item(s)");
        }

        return dependencies;
    }

    private async Task<List<string>> ValidateEquipmentDeleteAsync(long id, CancellationToken ct)
    {
        var dependencies = new List<string>();

        // Check for active exercises using this equipment
        var activeExercisesCount = await context.Exercise
            .Where(e => e.EquipmentId == id && !e.IsDeleted)
            .CountAsync(ct);

        if (activeExercisesCount > 0)
        {
            dependencies.Add($"{activeExercisesCount} active exercise(s)");
        }

        return dependencies;
    }

    private async Task<List<string>> ValidateWorkoutDeleteAsync(long id, CancellationToken ct)
    {
        var dependencies = new List<string>();

        // Check for active workout groups in this workout
        var activeWorkoutGroupsCount = await context.WorkoutGroup
            .Where(wg => wg.WorkoutId == id && !wg.IsDeleted)
            .CountAsync(ct);

        if (activeWorkoutGroupsCount > 0)
        {
            dependencies.Add($"{activeWorkoutGroupsCount} active workout group(s)");
        }

        // Check for active exercises in this workout
        var activeExercisesCount = await context.Exercise
            .Where(e => e.WorkoutId == id && !e.IsDeleted)
            .CountAsync(ct);

        if (activeExercisesCount > 0)
        {
            dependencies.Add($"{activeExercisesCount} active exercise(s)");
        }

        return dependencies;
    }

    private async Task<List<string>> ValidateWorkoutGroupDeleteAsync(long id, CancellationToken ct)
    {
        var dependencies = new List<string>();

        // Check for active exercises in this workout group
        var activeExercisesCount = await context.Exercise
            .Where(e => e.WorkoutGroupId == id && !e.IsDeleted)
            .CountAsync(ct);

        if (activeExercisesCount > 0)
        {
            dependencies.Add($"{activeExercisesCount} active exercise(s)");
        }

        return dependencies;
    }
}
