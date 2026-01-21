namespace FitNetClean.Application.Features.Equipment;

public record CreateEquipmentRequest(string Name, long CategoryId);

public record UpdateEquipmentRequest(string Name, long CategoryId);
