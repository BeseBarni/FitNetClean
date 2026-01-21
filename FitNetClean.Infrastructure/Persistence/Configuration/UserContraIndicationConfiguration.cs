using FitNetClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitNetClean.Infrastructure.Persistence.Configuration;

internal class UserAvoidedContraIndicationConfiguration : IEntityTypeConfiguration<UserAvoidedContraIndication>
{
    public void Configure(EntityTypeBuilder<UserAvoidedContraIndication> builder)
    {
        builder.HasKey(uci => new { uci.UserId, uci.ContraIndicationId });

        builder.Property(uci => uci.UserId)
            .IsRequired();

        builder.Property(uci => uci.ContraIndicationId)
            .IsRequired();

        builder.Property(uci => uci.MarkedAt)
            .IsRequired();

        builder.Property(uci => uci.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(uci => uci.User)
            .WithMany(u => u.AvoidedContraIndications)
            .HasForeignKey(uci => uci.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uci => uci.ContraIndication)
            .WithMany(ci => ci.AvoidedByUsers)
            .HasForeignKey(uci => uci.ContraIndicationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
