using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.ContraIndications;
using FitNetClean.Application.Features.ContraIndications.Queries;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using DomainPolicies = FitNetClean.Domain.Constants.Policies;
using DomainRoles = FitNetClean.Domain.Constants.Roles;

namespace FitNetClean.WebAPI.Endpoints.ContraIndications;

public class GetEquipmentAndExercisesByContraIndicationsEndpoint(IMediator mediator) 
    : Endpoint<GetEquipmentAndExercisesByContraIndicationsRequest, ApiResponse<ContraIndicationRelatedItemsDto>>
{
    public override void Configure()
    {
        Post("/contraindications/related-items");
        
        Policies(DomainPolicies.ActiveUsersOnly);
        Roles(DomainRoles.ProgramReader, DomainRoles.ProgramWriter, DomainRoles.Admin);
    }

    public override async Task HandleAsync(GetEquipmentAndExercisesByContraIndicationsRequest req, CancellationToken ct)
    {
        var query = new GetEquipmentAndExercisesByContraIndicationsQuery(req.ContraIndicationIds);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        Response = ApiResponse<ContraIndicationRelatedItemsDto>.Success(result, requestId);
    }
}
