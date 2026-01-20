using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.CodeName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Description)
            .HasMaxLength(500);

        builder.HasMany(w => w.WorkoutGroupList)
            .WithOne(wg => wg.Workout)
            .HasForeignKey(wg => wg.WorkoutId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.ExerciseList)
            .WithOne(e => e.Workout)
            .HasForeignKey(e => e.WorkoutId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
