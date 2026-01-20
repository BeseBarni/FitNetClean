using FitNetClean.Application.Common;
using MediatR;

namespace FitNetClean.Application.Features.Shared.Commands;

public record CreateCommand<T>(T Entity) : IRequest<T> where T : class;

public class CreateCommandHandler<T>(IFitnetContext context) : IRequestHandler<CreateCommand<T>, T> where T : class
{
    public async Task<T> Handle(CreateCommand<T> request, CancellationToken ct)
    {
        var entity = await context.Set<T>().AddAsync(request.Entity, ct);
        await context.SaveChangesAsync(ct);
        return entity.Entity;
    }
}
