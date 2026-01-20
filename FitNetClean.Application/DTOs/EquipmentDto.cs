namespace FitNetClean.Application.DTOs;

public record EquipmentDto(
    long Id,
    string Name,
    long CategoryId,
    string CategoryName
);
