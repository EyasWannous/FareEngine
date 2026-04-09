using FareEngine.Domain.FarePolicies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FareEngine.Infrastructure.Persistence.Configurations;

public sealed class FarePolicyConfiguration : IEntityTypeConfiguration<FarePolicy>
{
    public void Configure(EntityTypeBuilder<FarePolicy> builder)
    {
        builder.ToTable("FarePolicies").UseTphMappingStrategy();
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Type).IsRequired();
        
        builder.HasDiscriminator(p => p.Type)
            .HasValue<FlatRateFarePolicy>(FarePolicyType.FlatRate)
            .HasValue<DistanceBasedFarePolicy>(FarePolicyType.DistanceBased)
            .HasValue<ZoneBasedFarePolicy>(FarePolicyType.ZoneBased);
    }
}

public sealed class FlatRateFarePolicyConfiguration : IEntityTypeConfiguration<FlatRateFarePolicy>
{
    public void Configure(EntityTypeBuilder<FlatRateFarePolicy> builder)
    {
        builder.Property(p => p.FlatAmount)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}

public sealed class DistanceBasedFarePolicyConfiguration : IEntityTypeConfiguration<DistanceBasedFarePolicy>
{
    public void Configure(EntityTypeBuilder<DistanceBasedFarePolicy> builder)
    {
        builder.Property(p => p.RatePerKm)
            .IsRequired()
            .HasPrecision(18, 4);
    }
}

public sealed class ZoneBasedFarePolicyConfiguration : IEntityTypeConfiguration<ZoneBasedFarePolicy>
{
    public void Configure(EntityTypeBuilder<ZoneBasedFarePolicy> builder)
    {
        builder.Property(p => p.ZonePrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.ZoneNumber)
            .IsRequired();
    }
}