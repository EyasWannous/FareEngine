using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Infrastructure.FarePolicies;

public sealed class ZoneBasedFarePolicyCalculator : IFarePolicyCalculator
{
    public FareCalculationResult Calculate(FarePolicy farePolicy, SoldProduct soldProduct)
    {
        if (farePolicy is not ZoneBasedFarePolicy policy)
            throw new ArgumentException("Invalid fare policy type", nameof(farePolicy));
        
        if (soldProduct is not SoldHybridTrip product)
            throw new ArgumentException("Invalid sold product type", nameof(soldProduct));

        return new FareCalculationResult(
            Amount: policy.ZonePrice,
            Label: $"Zone-based fare (zone {product.ZoneNumber}): €{policy.ZonePrice}"
        );
    }
}