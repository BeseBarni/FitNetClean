using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.DTOs;

public record ExerciseDto(
    long Id,
    string Name,
    Measurement Repetition,
    long WorkoutId,
    long? WorkoutGroupId,
    long? EquipmentId
);
