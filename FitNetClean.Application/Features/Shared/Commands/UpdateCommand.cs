using FitNetClean.Application.Common;
using MediatR;

namespace FitNetClean.Application.Features.Shared.Commands;

public record UpdateCommand<T>(long Id, T Entity) : IRequest<T?> where T : class;

public class UpdateCommandHandler<T>(IFitnetContext context) : IRequestHandler<UpdateCommand<T>, T?> where T : class
{
    public async Task<T?> Handle(UpdateCommand<T> request, CancellationToken ct)
    {
        var existing = await context.Set<T>().FindAsync([request.Id], ct);
        if (existing == null)
            return null;

        var entry = context.Set<T>().Entry(existing);
        var sourceEntry = entry.Context.Entry(request.Entity);

        foreach (var property in entry.CurrentValues.Properties.Where(p => !p.IsKey()))
        {
            var sourceValue = sourceEntry.Property(property.Name).CurrentValue;
            entry.Property(property.Name).CurrentValue = sourceValue;
        }

        await context.SaveChangesAsync(ct);

        return existing;
    }
}
