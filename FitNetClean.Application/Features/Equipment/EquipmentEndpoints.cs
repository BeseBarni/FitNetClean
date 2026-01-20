using AutoMapper;
using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Equipment;

public class GetEquipmentListEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Domain.Entities.Equipment, EquipmentDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/equipment");
        AllowAnonymous();
    }
}

public class GetEquipmentByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Domain.Entities.Equipment, EquipmentDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/equipment/{id}");
        AllowAnonymous();
    }
}

public record CreateEquipmentRequest(string Name, long CategoryId);

public class CreateEquipmentEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Domain.Entities.Equipment, EquipmentDto, CreateEquipmentRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/equipment");
        AllowAnonymous();
    }
}

public record UpdateEquipmentRequest(string Name, long CategoryId);

public class UpdateEquipmentEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Domain.Entities.Equipment, EquipmentDto, UpdateEquipmentRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/equipment/{id}");
        AllowAnonymous();
    }
}

public class DeleteEquipmentEndpoint(IMediator mediator)
    : DeleteEndpointBase<Domain.Entities.Equipment>(mediator)
{
    public override void Configure()
    {
        Delete("/equipment/{id}");
        AllowAnonymous();
    }
}
