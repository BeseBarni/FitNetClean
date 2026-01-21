using FitNetClean.Application.Common;
using FitNetClean.Application.Common.Cache;
using FitNetClean.Application.Common.Interfaces;
using FitNetClean.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.ContraIndications.Queries;

public record GetEquipmentAndExercisesByContraIndicationsQuery(List<long> ContraIndicationIds) 
    : IRequest<ContraIndicationRelatedItemsDto>;

public class ContraIndicationRelatedItemsDto
{
    public List<EquipmentDto> Equipment { get; set; } = new();
    public List<ExerciseDto> Exercises { get; set; } = new();
}

public class GetEquipmentAndExercisesByContraIndicationsHandler 
    : IRequestHandler<GetEquipmentAndExercisesByContraIndicationsQuery, ContraIndicationRelatedItemsDto>
{
    private readonly IFitnetContext _context;
    private readonly AutoMapper.IMapper _mapper;
    private readonly ICacheService _cache;

    public GetEquipmentAndExercisesByContraIndicationsHandler(
        IFitnetContext context, 
        AutoMapper.IMapper mapper,
        ICacheService cache)
    {
        _context = context;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ContraIndicationRelatedItemsDto> Handle(
        GetEquipmentAndExercisesByContraIndicationsQuery request, 
        CancellationToken ct)
    {
        if (!request.ContraIndicationIds.Any())
            return new ContraIndicationRelatedItemsDto();

        // Create cache key from sorted IDs
        var sortedIds = string.Join(",", request.ContraIndicationIds.OrderBy(id => id));
        var cacheKey = CacheKeys.ContraIndicationEquipmentExercises(sortedIds);

        // Try to get from cache
        var result = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FetchFromDatabase(request.ContraIndicationIds, ct),
            TimeSpan.FromMinutes(30),
            ct
        );

        return result;
    }

    private async Task<ContraIndicationRelatedItemsDto> FetchFromDatabase(List<long> contraIndicationIds, CancellationToken ct)
    {
        var result = new ContraIndicationRelatedItemsDto();

        // Get Equipment that contains any of the contraindications
        var equipment = await _context.Equipment
            .Include(e => e.ContraIndicationList)
            .Include(e => e.Category)
            .Where(e => !e.IsDeleted && // ✅ Filter deleted equipment
                        !e.Category.IsDeleted && // ✅ Filter deleted categories
                        e.ContraIndicationList.Any(ci => !ci.IsDeleted && contraIndicationIds.Contains(ci.Id))) // ✅ Filter deleted CI
            .ToListAsync(ct);

        // Get Exercises that contains any of the contraindications
        var exercises = await _context.Exercise
            .Include(e => e.ContraIndicationList)
            .Where(e => !e.IsDeleted && // ✅ Filter deleted exercises
                        e.ContraIndicationList.Any(ci => !ci.IsDeleted && contraIndicationIds.Contains(ci.Id))) // ✅ Filter deleted CI
            .ToListAsync(ct);

        result.Equipment = _mapper.Map<List<EquipmentDto>>(equipment);
        result.Exercises = _mapper.Map<List<ExerciseDto>>(exercises);

        return result;
    }
}
