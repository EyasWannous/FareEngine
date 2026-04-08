using FareEngine.Domain.Modifications;

namespace FareEngine.Infrastructure.Modifications;

public sealed class FirstClassModificationCalculator : IModificationCalculator
{
    public ModificationResult Calculate(Modification modification, decimal currentFare)
    {
        if (modification is not FirstClassModification mod)
            throw new ArgumentException("Invalid modification type", nameof(modification));
    
        return new ModificationResult(mod.Surcharge, $"First class surcharge: €{mod.Surcharge}");
    }
}