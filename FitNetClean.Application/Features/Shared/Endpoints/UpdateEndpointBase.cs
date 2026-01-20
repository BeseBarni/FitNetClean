using AutoMapper;
using FastEndpoints;
using FitNetClean.Application.Features.Shared.Commands;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public class UpdateRequest<TUpdateDto>
{
    public long Id { get; set; }
    public TUpdateDto Data { get; set; } = default!;
}

public abstract class UpdateEndpointBase<TEntity, TDto, TUpdateDto> : Endpoint<UpdateRequest<TUpdateDto>, TDto?>
    where TEntity : class
    where TDto : class
    where TUpdateDto : class
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    protected UpdateEndpointBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task HandleAsync(UpdateRequest<TUpdateDto> req, CancellationToken ct)
    {
        var entity = _mapper.Map<TEntity>(req.Data);
        var updated = await _mediator.Send(new UpdateCommand<TEntity>(req.Id, entity), ct);
        
        if (updated == null)
        {
            HttpContext.Response.StatusCode = 404;
            return;
        }

        Response = _mapper.Map<TDto>(updated);
    }
}
