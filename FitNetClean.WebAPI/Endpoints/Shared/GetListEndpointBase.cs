using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.Shared.Queries;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public abstract class GetListEndpointBase<TEntity, TDto>(IMediator mediator, IMapper mapper) 
    : Endpoint<ListRequest, ApiResponse<List<TDto>>>
    where TEntity : class
    where TDto : class
{
    public override async Task HandleAsync(ListRequest req, CancellationToken ct)
    {
        var entities = await mediator.Send(new GetListQuery<TEntity>(req.IncludeDeleted), ct);
        var mapped = mapper.Map<List<TDto>>(entities);
        var requestId = HttpContext.GetRequestId();
        
        Response = ApiResponse<List<TDto>>.Success(mapped, requestId);
    }
}
