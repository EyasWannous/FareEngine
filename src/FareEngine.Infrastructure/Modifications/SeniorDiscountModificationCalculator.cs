using FareEngine.Domain.Modifications;

namespace FareEngine.Infrastructure.Modifications;

public sealed class SeniorDiscountModificationCalculator : IModificationCalculator
{
    public ModificationResult Calculate(Modification modification, decimal currentFare)
    {
        if (modification is not SeniorDiscountModification mod)
            throw new ArgumentException("Invalid modification type", nameof(modification));

        return new ModificationResult(
            Delta: -(mod.DiscountPercentage * currentFare),
            Label: $"Senior discount ({mod.DiscountPercentage * 100}%): -€{mod.DiscountPercentage * currentFare}"
        );
    }
}