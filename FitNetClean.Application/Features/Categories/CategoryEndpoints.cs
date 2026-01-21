using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.DTOs;
using FitNetClean.Application.Extensions;
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

    }
}

public class GetCategoryEquipmentEndpoint(IMediator mediator) : Endpoint<IdRequest, ApiResponse<CategoryEquipmentDto?>>
{
    public override void Configure()
    {
        Get("/categories/{id}/equipment");
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var query = new GetCategoryEquipmentQuery(req.Id);
        var result = await mediator.Send(query, ct);
        var requestId = HttpContext.GetRequestId();

        if (result == null)
        {
            Response = ApiResponse<CategoryEquipmentDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = ApiResponse<CategoryEquipmentDto?>.Success(result, requestId);
    }
}

public record CreateCategoryRequest(string Name);

public class CreateCategoryEndpoint(IMediator mediator, IMapper mapper)
    : CreateEndpointBase<Category, CategoryDto, CreateCategoryRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Post("/categories");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public record UpdateCategoryRequest(string Name);

public class UpdateCategoryEndpoint(IMediator mediator, IMapper mapper)
    : UpdateEndpointBase<Category, CategoryDto, UpdateCategoryRequest>(mediator, mapper)
{
    public override void Configure()
    {
        Put("/categories/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}

public class DeleteCategoryEndpoint(IMediator mediator)
    : DeleteEndpointBase<Category>(mediator)
{
    public override void Configure()
    {
        Delete("/categories/{id}");
        Policies(FitNetClean.Domain.Constants.Policies.AdminOnly);
    }
}
