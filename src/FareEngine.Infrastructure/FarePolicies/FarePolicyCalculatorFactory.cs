using FareEngine.Domain.FarePolicies;

namespace FareEngine.Infrastructure.FarePolicies;

public sealed class FarePolicyCalculatorFactory(
    FlatRateFarePolicyCalculator flatRate,
    DistanceBasedFarePolicyCalculator distanceBased,
    ZoneBasedFarePolicyCalculator zoneBased)
    : IFarePolicyCalculatorFactory
{
    public IFarePolicyCalculator Create(FarePolicyType farePolicyType)
    {
        return farePolicyType switch
        {
            FarePolicyType.FlatRate =>  flatRate,
            FarePolicyType.DistanceBased => distanceBased,
            FarePolicyType.ZoneBased => zoneBased,
            _ => throw new ArgumentOutOfRangeException(nameof(farePolicyType), farePolicyType, null)
        };
    }    
}