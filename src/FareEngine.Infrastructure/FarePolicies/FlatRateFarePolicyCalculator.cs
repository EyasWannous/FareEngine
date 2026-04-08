using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Infrastructure.FarePolicies;

public sealed class FlatRateFarePolicyCalculator : IFarePolicyCalculator
{
    public FareCalculationResult Calculate(FarePolicy farePolicy, SoldProduct soldProduct)
    {
        if (farePolicy is not FlatRateFarePolicy policy)
            throw new ArgumentException("Invalid fare policy type", nameof(farePolicy));

        return new FareCalculationResult(
            Amount: policy.FlatAmount,
            Label: $"Flat rate fare: €{policy.FlatAmount}"
        );
    }
}