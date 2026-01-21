using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Equipment;
using FitNetClean.WebAPI.Endpoints.Shared;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Equipment;

public class GetEquipmentListEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Domain.Entities.Equipment, EquipmentDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/equipment");

    }
}

public class GetEquipmentByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Domain.Entities.Equipment, EquipmentDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/equipment/{id}");

    }
}

public class CreateEquipmentEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Domain.Entities.Equipment, EquipmentDto, CreateEquipmentRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/equipment");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class UpdateEquipmentEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Domain.Entities.Equipment, EquipmentDto, UpdateEquipmentRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/equipment/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class DeleteEquipmentEndpoint(IMediator mediator)
    : DeleteEndpointBase<Domain.Entities.Equipment>(mediator)
{
    public override void Configure()
    {
        Delete("/equipment/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}
