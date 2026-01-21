using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Extensions;
using FitNetClean.Application.Features.Shared.Queries;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public class IdRequest
{
    public long Id { get; set; }
}

public abstract class GetByIdEndpointBase<TEntity, TDto> : Endpoint<IdRequest, ApiResponse<TDto?>>
    where TEntity : class
    where TDto : class
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    protected GetByIdEndpointBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        var entity = await _mediator.Send(new GetByIdQuery<TEntity>(req.Id), ct);
        var requestId = HttpContext.GetRequestId();
        
        if (entity == null)
        {
            Response = ApiResponse<TDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        var mapped = _mapper.Map<TDto>(entity);
        Response = ApiResponse<TDto?>.Success(mapped, requestId);
    }
}
