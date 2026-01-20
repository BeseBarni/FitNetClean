using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Exceptions;
using FitNetClean.Application.Common.Validation;
using FitNetClean.Domain.Common;
using MediatR;

namespace FitNetClean.Application.Features.Shared.Commands;

public record DeleteCommand<T>(long Id, bool HardDelete = false) : IRequest<bool> where T : class;

public class DeleteCommandHandler<T>(IFitnetContext context, IDeleteValidator deleteValidator)
    : IRequestHandler<DeleteCommand<T>, bool> where T : class
{
    public async Task<bool> Handle(DeleteCommand<T> request, CancellationToken ct)
    {
        var entity = await context.Set<T>().FindAsync([request.Id], ct);
        if (entity == null)
            return false;

        if (!request.HardDelete)
        {
            var (canDelete, dependencies) = await deleteValidator.ValidateDeleteAsync<T>(request.Id, ct);

            if (!canDelete)
            {
                var entityName = typeof(T).Name;
                throw new DeleteValidationException(entityName, dependencies);
            }
        }

        if (request.HardDelete)
        {
            context.Set<T>().Remove(entity);
        }
        else if (entity is IDeletable deletable)
        {
            deletable.IsDeleted = true;
        }
        else
        {
            // If entity doesn't support soft delete, perform hard delete
            context.Set<T>().Remove(entity);
        }

        await context.SaveChangesAsync(ct);
        return true;
    }
}
