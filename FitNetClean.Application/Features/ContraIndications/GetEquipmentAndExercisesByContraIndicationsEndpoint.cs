using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.ContraIndications.Queries;
using FitNetClean.Domain.Constants;
using MediatR;

namespace FitNetClean.Application.Features.ContraIndications;

public record GetEquipmentAndExercisesByContraIndicationsRequest
{
    public List<long> ContraIndicationIds { get; init; } = new();
}

public class GetEquipmentAndExercisesByContraIndicationsEndpoint 
    : Endpoint<GetEquipmentAndExercisesByContraIndicationsRequest, ApiResponse<ContraIndicationRelatedItemsDto>>
{
    private readonly IMediator _mediator;

    public GetEquipmentAndExercisesByContraIndicationsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/contraindications/related-items");
        
        Policies(Domain.Constants.Policies.ActiveUsersOnly);
        Roles(Domain.Constants.Roles.ProgramReader, Domain.Constants.Roles.ProgramWriter, Domain.Constants.Roles.Admin);
    }

    public override async Task HandleAsync(GetEquipmentAndExercisesByContraIndicationsRequest req, CancellationToken ct)
    {
        var query = new GetEquipmentAndExercisesByContraIndicationsQuery(req.ContraIndicationIds);
        var result = await _mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<ContraIndicationRelatedItemsDto>.Success(result, requestId);
    }
}
