namespace FitNetClean.Application.Features.Users;

public record UpdateUserAvoidedContraIndicationsRequest
{
    public List<long> ContraIndicationIds { get; init; } = new();
}
