using FitNetClean.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Exercises.Commands;

public record RemoveContraIndicationFromExerciseCommand(long ExerciseId, long ContraIndicationId) : IRequest<bool>;

public class RemoveContraIndicationFromExerciseHandler(IFitnetContext context) 
    : IRequestHandler<RemoveContraIndicationFromExerciseCommand, bool>
{
    public async Task<bool> Handle(RemoveContraIndicationFromExerciseCommand request, CancellationToken ct)
    {
        var exercise = await context.Exercise
            .Include(e => e.ContraIndicationList)
            .FirstOrDefaultAsync(e => e.Id == request.ExerciseId, ct);

        if (exercise == null)
            return false;

        var contraIndication = exercise.ContraIndicationList
            .FirstOrDefault(ci => ci.Id == request.ContraIndicationId);

        if (contraIndication == null)
            return false;

        exercise.ContraIndicationList.Remove(contraIndication);
        await context.SaveChangesAsync(ct);

        return true;
    }
}
