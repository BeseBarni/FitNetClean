using FastEndpoints;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Features.Categories.Queries;
using FitNetClean.Application.Features.Shared.Endpoints;
using FitNetClean.Domain.Entities;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Categories;

public class GetCategoriesEndpoint(IMediator mediator, IMapper mapper)
    : GetListEndpointBase<Category, CategoryDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/categories");
        AllowAnonymous();
    }
}

public class GetCategoryByIdEndpoint(IMediator mediator, IMapper mapper)
    : GetByIdEndpointBase<Category, CategoryDto>(mediator, mapper)
{
    public override void Configure()
    {
        Get("/categories/{id}");
        AllowAnonymous();
    }
}

public class GetCategoryEquipmentEndpoint(IMediator mediator) : Endpoint<IdRequest, CategoryEquipmentDto?>
{
    public override void Configure()
    {
        Get("/categories/{id}/equipment");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetCategoryEquipmentQuery(req.Id);
        var result = await mediator.Send(query, ct);

        if (result == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = result;
    }
}

public record CreateCategoryRequest(string Name);

public class CreateCategoryEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Category, CategoryDto, CreateCategoryRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/categories");
        AllowAnonymous();
    }
}

public record UpdateCategoryRequest(string Name);

public class UpdateCategoryEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Category, CategoryDto, UpdateCategoryRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/categories/{id}");
        AllowAnonymous();
    }
}

public class DeleteCategoryEndpoint(IMediator mediator)
    : DeleteEndpointBase<Category>(mediator)
{
    public override void Configure()
    {
        Delete("/categories/{id}");
        AllowAnonymous();
    }
}
