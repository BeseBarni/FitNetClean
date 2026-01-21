using FitNetClean.Application.Common;
using FitNetClean.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Users.Queries;

public record GetUserAvoidedContraIndicationsQuery(long UserId) : IRequest<List<ContraIndicationDto>>;

public class GetUserAvoidedContraIndicationsHandler : IRequestHandler<GetUserAvoidedContraIndicationsQuery, List<ContraIndicationDto>>
{
    private readonly IFitnetContext _context;
    private readonly AutoMapper.IMapper _mapper;

    public GetUserAvoidedContraIndicationsHandler(IFitnetContext context, AutoMapper.IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ContraIndicationDto>> Handle(GetUserAvoidedContraIndicationsQuery request, CancellationToken ct)
    {
        var contraIndications = await _context.UserAvoidedContraIndication
            .Where(uci => uci.UserId == request.UserId && !uci.IsDeleted)
            .Select(uci => uci.ContraIndication)
            .ToListAsync(ct);

        return _mapper.Map<List<ContraIndicationDto>>(contraIndications);
    }
}
