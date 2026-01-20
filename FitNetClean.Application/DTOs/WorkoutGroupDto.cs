namespace FitNetClean.Application.DTOs;

public record WorkoutGroupDto(
    long Id,
    long WorkoutId,
    string Title
);
