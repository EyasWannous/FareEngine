using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Infrastructure.FarePolicies;

public sealed class DistanceBasedFarePolicyCalculator : IFarePolicyCalculator
{
    public FareCalculationResult Calculate(FarePolicy farePolicy, SoldProduct soldProduct)
    {
        if (farePolicy is not DistanceBasedFarePolicy policy)
            throw new ArgumentException("Invalid fare policy type", nameof(farePolicy));
        
        if (soldProduct is not SoldHybridTrip product)
            throw new ArgumentException("Invalid sold product type", nameof(soldProduct));
        
        var amount = policy.RatePerKm * product.DistanceInKm;
        
        return new FareCalculationResult(
            Amount: amount,
            Label: $"Distance-based fare ({product.DistanceInKm}km x €{policy.RatePerKm}/km)"
        );
    }
}