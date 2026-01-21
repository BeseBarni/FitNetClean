using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class FavoriteWorkoutConfiguration : IEntityTypeConfiguration<FavoriteWorkout>
{
    public void Configure(EntityTypeBuilder<FavoriteWorkout> builder)
    {
        builder.HasKey(fw => new { fw.UserId, fw.WorkoutId });

        builder.Property(fw => fw.UserId)
            .IsRequired();

        builder.Property(fw => fw.WorkoutId)
            .IsRequired();

        builder.Property(fw => fw.FavoritedAt)
            .IsRequired();

        builder.Property(fw => fw.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(fw => fw.User)
            .WithMany(u => u.FavoriteWorkouts)
            .HasForeignKey(fw => fw.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(fw => fw.Workout)
            .WithMany(w => w.FavoritedBy)
            .HasForeignKey(fw => fw.WorkoutId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

