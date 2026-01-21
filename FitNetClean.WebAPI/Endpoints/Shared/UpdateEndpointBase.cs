using FastEndpoints;
using FitNetClean.Application.Common.Models;
using FitNetClean.Application.Features.Shared.Commands;
using FitNetClean.WebAPI.Extensions;
using MediatR;
using IMapper = AutoMapper.IMapper;

namespace FitNetClean.WebAPI.Endpoints.Shared;

public class UpdateRequest<TUpdateDto>
{
    public long Id { get; set; }
    public TUpdateDto Data { get; set; } = default!;
}

public abstract class UpdateEndpointBase<TEntity, TDto, TUpdateDto>(IMediator mediator, IMapper mapper) 
    : Endpoint<UpdateRequest<TUpdateDto>, ApiResponse<TDto?>>
    where TEntity : class
    where TDto : class
    where TUpdateDto : class
{
    public override async Task HandleAsync(UpdateRequest<TUpdateDto> req, CancellationToken ct)
    {
        var entity = mapper.Map<TEntity>(req.Data);
        var updated = await mediator.Send(new UpdateCommand<TEntity>(req.Id, entity), ct);
        var requestId = HttpContext.GetRequestId();
        
        if (updated == null)
        {
            Response = ApiResponse<TDto?>.NotFound(requestId);
            HttpContext.Response.StatusCode = 404;
            return;
        }

        var mapped = mapper.Map<TDto>(updated);
        Response = ApiResponse<TDto?>.Success(mapped, requestId);
    }
}
