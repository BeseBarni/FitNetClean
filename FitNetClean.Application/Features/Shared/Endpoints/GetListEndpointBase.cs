using FastEndpoints;
using FitNetClean.Application.Common;
using FitNetClean.Application.Features.Shared.Queries;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.Application.Features.Shared.Endpoints;

public abstract class GetListEndpointBase<TEntity, TDto> : Endpoint<ListRequest, List<TDto>>
    where TEntity : class
    where TDto : class
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    protected GetListEndpointBase(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task HandleAsync(ListRequest req, CancellationToken ct)
    {
        var entities = await _mediator.Send(new GetListQuery<TEntity>(req.IncludeDeleted), ct);
        Response = _mapper.Map<List<TDto>>(entities);
    }
}
