using FitNetClean.Application.Common;
using FitNetClean.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitNetClean.Application.Features.Users.Commands;

public record UpdateUserAvoidedContraIndicationsCommand(long UserId, List<long> ContraIndicationIds) : IRequest<bool>;

public class UpdateUserAvoidedContraIndicationsHandler : IRequestHandler<UpdateUserAvoidedContraIndicationsCommand, bool>
{
    private readonly IFitnetContext _context;

    public UpdateUserAvoidedContraIndicationsHandler(IFitnetContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateUserAvoidedContraIndicationsCommand request, CancellationToken ct)
    {
        // Check if user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == request.UserId, ct);
        if (!userExists)
            return false;

        // Validate all contraindication IDs exist
        if (request.ContraIndicationIds.Any())
        {
            var validIds = await _context.ContraIndication
                .Where(ci => request.ContraIndicationIds.Contains(ci.Id))
                .Select(ci => ci.Id)
                .ToListAsync(ct);

            if (validIds.Count != request.ContraIndicationIds.Distinct().Count())
                return false; // Some IDs are invalid
        }

        // Get all existing user avoided contraindications (including soft deleted)
        var existingRecords = await _context.UserAvoidedContraIndication
            .IgnoreQueryFilters()
            .Where(uci => uci.UserId == request.UserId)
            .ToListAsync(ct);

        var requestedIds = request.ContraIndicationIds.Distinct().ToHashSet();

        // Process existing records
        foreach (var existing in existingRecords)
        {
            if (requestedIds.Contains(existing.ContraIndicationId))
            {
                // Should be active - restore if soft deleted
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.MarkedAt = DateTime.UtcNow;
                }
                // Remove from set so we don't create a duplicate
                requestedIds.Remove(existing.ContraIndicationId);
            }
            else
            {
                // Not in the requested list - soft delete if active
                if (!existing.IsDeleted)
                {
                    existing.IsDeleted = true;
                }
            }
        }

        // Create new records for IDs that don't exist yet
        foreach (var contraIndicationId in requestedIds)
        {
            var newRecord = new UserAvoidedContraIndication
            {
                UserId = request.UserId,
                ContraIndicationId = contraIndicationId,
                MarkedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.UserAvoidedContraIndication.Add(newRecord);
        }

        await _context.SaveChangesAsync(ct);
        return true;
    }
}
