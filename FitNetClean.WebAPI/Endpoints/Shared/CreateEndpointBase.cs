using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.Shared.Commands;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public abstract class CreateEndpointBase<TEntity, TDto, TCreateDto>(IMediator mediator, IMapper mapper) 
    : Endpoint<TCreateDto, ApiResponse<TDto>>
    where TEntity : class
    where TDto : class
    where TCreateDto : class
{
    public override async Task HandleAsync(TCreateDto req, CancellationToken ct)
    {
        var entity = mapper.Map<TEntity>(req);
        var created = await mediator.Send(new CreateCommand<TEntity>(entity), ct);
        var mapped = mapper.Map<TDto>(created);
        var requestId = HttpContext.GetRequestId();
        
        Response = ApiResponse<TDto>.Success(mapped, requestId, 201);
        HttpContext.Response.StatusCode = 201;
    }
}
