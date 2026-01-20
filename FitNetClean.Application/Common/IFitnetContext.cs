using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FitNetClean.Application.Common;

public interface IFitnetContext
{
    DbSet<Category> Category { get; set; }
    DbSet<ContraIndication> ContraIndication { get; set; }
    DbSet<Equipment> Equipment { get; set; }
    DbSet<Exercise> Exercise { get; set; }
    DbSet<Measurement> Measurement { get; set; }
    DbSet<Workout> Workout { get; set; }
    DbSet<WorkoutGroup> WorkoutGroup { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
