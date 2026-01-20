namespace FitNetClean.Application.DTOs;

public record CategoryEquipmentDto(
    long Id,
    string Name,
    ICollection<EquipmentDto> Equipment
);
