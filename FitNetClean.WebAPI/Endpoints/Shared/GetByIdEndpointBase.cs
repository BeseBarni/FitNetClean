using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.Shared.Queries;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public abstract class GetByIdEndpointBase<TEntity, TDto>(IMediator mediator, IMapper mapper) 
    : Endpoint<IdRequest, ApiResponse<TDto?>>
    where TEntity : class
    where TDto : class
{
    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var entity = await mediator.Send(new GetByIdQuery<TEntity>(req.Id), ct);
        var requestId = HttpContext.GetRequestId();
        
        if (entity == null)
        {
            Response = ApiResponse<TDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        var mapped = mapper.Map<TDto>(entity);
        Response = ApiResponse<TDto?>.Success(mapped, requestId);
    }
}
