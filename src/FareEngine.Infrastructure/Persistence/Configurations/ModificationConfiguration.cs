using FareEngine.Domain.Modifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FareEngine.Infrastructure.Persistence.Configurations;

public sealed class ModificationConfiguration : IEntityTypeConfiguration<Modification>
{
    public void Configure(EntityTypeBuilder<Modification> builder)
    {
        builder.ToTable("Modifications").UseTphMappingStrategy();
        
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Type).IsRequired();
        
        builder.HasDiscriminator(m => m.Type)
            .HasValue<FirstClassModification>(ModificationType.FirstClass)
            .HasValue<SeniorDiscountModification>(ModificationType.SeniorDiscount);
    }
}

public sealed class FirstClassModificationConfiguration : IEntityTypeConfiguration<FirstClassModification>
{
    public void Configure(EntityTypeBuilder<FirstClassModification> builder)
    {
        builder.Property(m => m.Surcharge).IsRequired();
    }
}

public sealed class SeniorDiscountModificationConfiguration : IEntityTypeConfiguration<SeniorDiscountModification>
{
    public void Configure(EntityTypeBuilder<SeniorDiscountModification> builder)
    {
        builder.Property(m => m.DiscountPercentage).IsRequired();
    }
}