namespace FitNetClean.Application.DTOs;

public record WorkoutDto(
    long Id,
    string CodeName,
    string Title,
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes,
    int TotalDurationMinutes
);
