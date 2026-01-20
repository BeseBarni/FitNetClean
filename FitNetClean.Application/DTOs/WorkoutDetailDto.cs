namespace FitNetClean.Application.DTOs;

public record WorkoutDetailDto(
    long Id,
    string CodeName,
    string Title,
    string? Description,
    int WarmupDurationMinutes,
    int MainWorkoutDurationMinutes,
    int TotalDurationMinutes,
    ICollection<WorkoutGroupDetailDto> WorkoutGroups
);

public record WorkoutGroupDetailDto(
    long Id,
    string Title,
    ICollection<ExerciseDetailDto> Exercises
);

public record ExerciseDetailDto(
    long Id,
    string Name,
    string Repetition,
    EquipmentDetailDto? Equipment,
    ICollection<ContraIndicationDetailDto> ContraIndications
);

public record EquipmentDetailDto(
    long Id,
    string Name,
    string CategoryName
);

public record ContraIndicationDetailDto(
    long Id,
    string Name
);
