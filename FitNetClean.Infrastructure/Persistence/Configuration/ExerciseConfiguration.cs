using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(e => e.Repetition, r =>
        {
            r.Property(m => m.Quantity).IsRequired();
            r.Property(m => m.Unit).IsRequired();
        });

        builder.HasOne(e => e.Equipment)
            .WithMany(eq => eq.ExerciseList)
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(e => e.Workout)
            .WithMany(w => w.ExerciseList)
            .HasForeignKey(e => e.WorkoutId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.WorkoutGroup)
            .WithMany(wg => wg.ExerciseList)
            .HasForeignKey(e => e.WorkoutGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(e => e.ContraIndicationList)
            .WithMany(c => c.ExerciseList);
    }
}
