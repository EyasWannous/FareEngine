using FareEngine.Domain.FarePolicies;

namespace FareEngine.Infrastructure.FarePolicies;

public sealed class FarePolicyCalculatorFactory : IFarePolicyCalculatorFactory
{
    private readonly FlatRateFarePolicyCalculator _flatRate;
    private readonly DistanceBasedFarePolicyCalculator _distanceBased;
    private readonly ZoneBasedFarePolicyCalculator _zoneBased;

    public FarePolicyCalculatorFactory(
        FlatRateFarePolicyCalculator flatRate,
        DistanceBasedFarePolicyCalculator distanceBased,
        ZoneBasedFarePolicyCalculator zoneBased)
    {
        _flatRate = flatRate;
        _distanceBased = distanceBased;
        _zoneBased = zoneBased;
    }
    
    public IFarePolicyCalculator Create(FarePolicyType farePolicyType)
    {
        return farePolicyType switch
        {
            FarePolicyType.FlatRate =>  _flatRate,
            FarePolicyType.DistanceBased => _distanceBased,
            FarePolicyType.ZoneBased => _zoneBased,
            _ => throw new ArgumentOutOfRangeException(nameof(farePolicyType), farePolicyType, null)
        };
    }    
}