using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Exercises.Commands;

public record AddContraIndicationToExerciseCommand(long ExerciseId, long ContraIndicationId) : IRequest<bool>;

public class AddContraIndicationToExerciseHandler(IFitnetContext context) 
    : IRequestHandler<AddContraIndicationToExerciseCommand, bool>
{
    public async Task<bool> Handle(AddContraIndicationToExerciseCommand request, CancellationToken ct)
    {
        var exercise = await context.Exercise
            .Include(e => e.ContraIndicationList)
            .FirstOrDefaultAsync(e => e.Id == request.ExerciseId, ct);

        if (exercise == null)
            return false;

        var contraIndication = await context.ContraIndication
            .FirstOrDefaultAsync(ci => ci.Id == request.ContraIndicationId, ct);

        if (contraIndication == null)
            return false;

        if (exercise.ContraIndicationList.Any(ci => ci.Id == request.ContraIndicationId))
            return true;

        exercise.ContraIndicationList.Add(contraIndication);
        await context.SaveChangesAsync(ct);

        return true;
    }
}
