using FareEngine.Domain.SoldProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FareEngine.Infrastructure.Persistence.Configurations;

public sealed class SoldProductConfiguration : IEntityTypeConfiguration<SoldProduct>
{
    public void Configure(EntityTypeBuilder<SoldProduct> builder)
    {
        builder.ToTable("SoldProducts").UseTphMappingStrategy();
        
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Type).IsRequired();
        
        // builder.Metadata.FindNavigation(nameof(SoldProduct.FarePolicies))?.SetPropertyAccessMode(PropertyAccessMode.Field);
        // builder.Metadata.FindNavigation(nameof(SoldProduct.Modifications))?.SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.OwnsMany(p => p.FarePolicies);
        builder.OwnsMany(p => p.Modifications);
        
        builder.HasDiscriminator(p => p.Type)
            .HasValue<SoldDailyPass>(ProductType.DailyPass)
            .HasValue<SoldHybridTrip>(ProductType.Hybrid);

    }
}

public sealed class SoldDailyPassConfiguration : IEntityTypeConfiguration<SoldDailyPass>
{
    public void Configure(EntityTypeBuilder<SoldDailyPass> builder)
    {
    }
}

public sealed class SoldHybridTripConfiguration : IEntityTypeConfiguration<SoldHybridTrip>
{
    public void Configure(EntityTypeBuilder<SoldHybridTrip> builder)
    {
        builder.Property(p => p.DistanceInKm).IsRequired();
        builder.Property(p => p.ZoneNumber).IsRequired();
    }
}