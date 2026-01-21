namespace FitNetClean.Application.Features.ContraIndications;

public record CreateContraIndicationRequest(string Name);

public record UpdateContraIndicationRequest(string Name);

public record GetEquipmentAndExercisesByContraIndicationsRequest
{
    public List<long> ContraIndicationIds { get; init; } = new();
}
