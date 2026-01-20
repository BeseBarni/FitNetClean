using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Shared.Queries;

public record GetByIdQuery<T>(long Id, bool IncludeDeleted = false) : IRequest<T?> where T : class;

public class GetByIdHandler<T>(IFitnetContext context) : IRequestHandler<GetByIdQuery<T>, T?> where T : class
{
    public async Task<T?> Handle(GetByIdQuery<T> request, CancellationToken ct)
    {
        var query = context.Set<T>().AsNoTracking();

        if (request.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == request.Id, ct);
    }
}
