using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class ContraIndicationConfiguration : IEntityTypeConfiguration<ContraIndication>
{
    public void Configure(EntityTypeBuilder<ContraIndication> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(c => c.ExerciseList)
            .WithMany(e => e.ContraIndicationList);

        builder.HasMany(c => c.EquipmentList)
            .WithMany(e => e.ContraIndicationList);
    }
}
