using AutoMapper;
using FitNetClean.Application.Common;
using FitNetClean.Application.DTOs;
using FitNetClean.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Workouts.Queries;

public record GetWorkoutsForUserQuery(long? UserId, bool IncludeDeleted = false) : IRequest<List<WorkoutDto>>;

public class GetWorkoutsForUserHandler : IRequestHandler<GetWorkoutsForUserQuery, List<WorkoutDto>>
{
    private readonly IFitnetContext _context;
    private readonly IMapper _mapper;

    public GetWorkoutsForUserHandler(IFitnetContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkoutDto>> Handle(GetWorkoutsForUserQuery request, CancellationToken ct)
    {
        var query = _context.Workout.AsNoTracking();

        if (request.IncludeDeleted)
        {
            query = query.IgnoreQueryFilters();
        }

        var workouts = await query.ToListAsync(ct);

        // Ha nincs UserId, egyszerű mapping
        if (!request.UserId.HasValue)
        {
            return _mapper.Map<List<WorkoutDto>>(workouts);
        }

        // User által kerülendő kontraindikációk lekérése
        var userAvoidedContraIndicationIds = await _context.UserAvoidedContraIndication
            .Where(uci => uci.UserId == request.UserId.Value && !uci.IsDeleted)
            .Select(uci => uci.ContraIndicationId)
            .ToListAsync(ct);

        // Ha nincs kerülendő kontraindikáció, egyszerű mapping
        if (!userAvoidedContraIndicationIds.Any())
        {
            return _mapper.Map<List<WorkoutDto>>(workouts);
        }

        // Workoutok kontraindikációinak lekérése (Exercises és Equipment)
        var workoutIds = workouts.Select(w => w.Id).ToList();

        // Kontraindikációk az exercisekből
        var exerciseContraIndications = await _context.Exercise
            .Where(e => workoutIds.Contains(e.WorkoutId) && !e.IsDeleted) // ✅ Filter deleted exercises
            .SelectMany(e => e.ContraIndicationList
                .Where(ci => !ci.IsDeleted) // ✅ Filter deleted contraindications
                .Select(ci => new { WorkoutId = e.WorkoutId, ContraIndicationId = ci.Id }))
            .ToListAsync(ct);

        // Kontraindikációk az equipmentekből (exerciseken keresztül)
        var equipmentContraIndications = await _context.Exercise
            .Where(e => workoutIds.Contains(e.WorkoutId) && e.EquipmentId != null && !e.IsDeleted) // ✅ Filter deleted exercises
            .Include(e => e.Equipment)
            .ThenInclude(eq => eq!.ContraIndicationList)
            .Where(e => e.Equipment != null && !e.Equipment.IsDeleted) // ✅ Filter deleted equipment
            .SelectMany(e => e.Equipment!.ContraIndicationList
                .Where(ci => !ci.IsDeleted) // ✅ Filter deleted contraindications
                .Select(ci => new { WorkoutId = e.WorkoutId, ContraIndicationId = ci.Id }))
            .ToListAsync(ct);

        // Összes kontraindikáció workout szerint csoportosítva
        var allContraIndications = exerciseContraIndications
            .Concat(equipmentContraIndications)
            .GroupBy(x => x.WorkoutId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ContraIndicationId).Distinct().ToList());

        // WorkoutDto-k létrehozása a flag-gel
        var workoutDtos = new List<WorkoutDto>();
        foreach (var workout in workouts)
        {
            var dto = _mapper.Map<WorkoutDto>(workout);
            
            // Ellenőrizzük, hogy a workout tartalmaz-e kerülendő kontraindikációt
            var containsAvoided = false;
            if (allContraIndications.TryGetValue(workout.Id, out var contraIndicationIds))
            {
                containsAvoided = contraIndicationIds.Any(ciId => userAvoidedContraIndicationIds.Contains(ciId));
            }

            // Új DTO létrehozása a flag-gel
            workoutDtos.Add(dto with { ContainsAvoidedContraIndications = containsAvoided });
        }

        return workoutDtos;
    }
}
