using FitNetClean.Domain.Entities;

namespace FitNetClean.Application.Features.Exercises;

public record CreateExerciseRequest(
    string Name,
    Measurement Repetition,
    long WorkoutId,
    long? WorkoutGroupId,
    long? EquipmentId);

public record UpdateExerciseRequest(
    string Name,
    Measurement Repetition,
    long WorkoutId,
    long? WorkoutGroupId,
    long? EquipmentId);

public record AddContraIndicationRequest
{
    public long ContraIndicationId { get; init; }
}
