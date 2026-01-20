namespace FitNetClean.Application.DTOs;

public record ContraIndicationRelatedDto(
    long Id,
    string Name,
    ICollection<ExerciseDto> Exercises,
    ICollection<EquipmentDto> Equipment
);
