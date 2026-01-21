namespace FitNetClean.Application.Features.WorkoutGroups;

public record CreateWorkoutGroupRequest(long WorkoutId, string Title);

public record UpdateWorkoutGroupRequest(long WorkoutId, string Title);

public record AddExerciseRequest
{
    public long ExerciseId { get; init; }
}
