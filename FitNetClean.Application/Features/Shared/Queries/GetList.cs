using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Shared.Queries;

public record GetListQuery<T>(bool IncludeDeleted = false) : IRequest<List<T>> where T : class;

public class GetListHandler<T>(IFitnetContext context)
    : IRequestHandler<GetListQuery<T>, List<T>> where T : class
{
    public async Task<List<T>> Handle(GetListQuery<T> request, CancellationToken ct)
    {
        var query = context.Set<T>().AsNoTracking();

        if (request.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.ToListAsync(ct);
    }
}
