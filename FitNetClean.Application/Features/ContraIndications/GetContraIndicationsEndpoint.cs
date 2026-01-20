using AutoMapper;
using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.ContraIndications.Queries;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.ContraIndications;

public class GetContraIndicationsEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<ContraIndication, ContraIndicationDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/contraindications");
        AllowAnonymous();
    }
}

public class GetContraIndicationByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<ContraIndication, ContraIndicationDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/contraindications/{id}");
        AllowAnonymous();
    }
}

public class GetContraIndicationRelatedEndpoint(IMediator mediator) : Endpoint<IdRequest, ContraIndicationRelatedDto?>
{
    public override void Configure()
    {
        Get("/contraindications/{id}/related");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetContraIndicationRelatedQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public record CreateContraIndicationRequest(string Name);

public class CreateContraIndicationEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<ContraIndication, ContraIndicationDto, CreateContraIndicationRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/contraindications");
        AllowAnonymous();
    }
}

public record UpdateContraIndicationRequest(string Name);

public class UpdateContraIndicationEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<ContraIndication, ContraIndicationDto, UpdateContraIndicationRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/contraindications/{id}");
        AllowAnonymous();
    }
}

public class DeleteContraIndicationEndpoint(IMediator mediator)
    : DeleteEndpointBase<ContraIndication>(mediator)
{
    public override void Configure()
    {
        Delete("/contraindications/{id}");
        AllowAnonymous();
    }
}
