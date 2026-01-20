using FastEndpoints;
using FitNetClean.Application.Features.Shared.Queries;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public class IdRequest
{
    public long Id { get; set; }
}

public abstract class GetByIdEndpointBase<TEntity, TDto> : Endpoint<IdRequest, TDto?>
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
        
        if (entity == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = _mapper.Map<TDto>(entity);
    }
}
