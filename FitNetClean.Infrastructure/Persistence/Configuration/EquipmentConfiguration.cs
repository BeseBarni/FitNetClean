using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.EquipmentList)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ExerciseList)
            .WithOne(ex => ex.Equipment)
            .HasForeignKey(ex => ex.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ContraIndicationList)
            .WithMany(c => c.EquipmentList);
    }
}
