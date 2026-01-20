using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class WorkoutGroupConfiguration : IEntityTypeConfiguration<WorkoutGroup>
{
    public void Configure(EntityTypeBuilder<WorkoutGroup> builder)
    {
        builder.HasKey(wg => wg.Id);

        builder.Property(wg => wg.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(wg => wg.Workout)
            .WithMany(w => w.WorkoutGroupList)
            .HasForeignKey(wg => wg.WorkoutId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(wg => wg.ExerciseList)
            .WithOne(e => e.WorkoutGroup)
            .HasForeignKey(e => e.WorkoutGroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
