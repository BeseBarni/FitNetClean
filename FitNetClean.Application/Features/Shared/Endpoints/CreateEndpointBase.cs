using FastEndpoints;
using FitNetClean.Application.Features.Shared.Commands;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public abstract class CreateEndpointBase<TEntity, TDto, TCreateDto> : Endpoint<TCreateDto, TDto>
    where TEntity : class
    where TDto : class
    where TCreateDto : class
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    protected CreateEndpointBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task HandleAsync(TCreateDto req, CancellationToken ct)
    {
        var entity = _mapper.Map<TEntity>(req);
        var created = await _mediator.Send(new CreateCommand<TEntity>(entity), ct);
        
        Response = _mapper.Map<TDto>(created);
        
        // 201 Created status
        HttpContext.Response.StatusCode = 201;
    }
}
