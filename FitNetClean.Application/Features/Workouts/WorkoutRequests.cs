namespace FitNetClean.Application.Features.Workouts;

public record CreateWorkoutRequest(
    string CodeName,
    string Title,
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes);

public record UpdateWorkoutRequest(
    string CodeName,
    string Title,
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes);

public record AddWorkoutGroupRequest
{
    public long WorkoutGroupId { get; init; }
}

public record FavoriteWorkoutRequest
{
    public long UserId { get; init; }
}

public record GetWorkoutsWithContraIndicationsRequest
{
    public long? UserId { get; init; }
    public bool IncludeDeleted { get; init; } = false;
}
