namespace FitNetClean.Application.DTOs;

public record ExerciseWorkoutProgramsDto(
    long ExerciseId,
    string ExerciseName,
    ICollection<WorkoutProgramSummaryDto> Programs
);

public record WorkoutProgramSummaryDto(
    long Id,
    string CodeName,
    string Title,
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes,
    int TotalDurationMinutes,
    bool IsDeleted,
    ICollection<ContraIndicationDetailDto> ContraIndications
);
