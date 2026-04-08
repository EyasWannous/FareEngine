namespace FareEngine.Domain.FarePolicies;

public sealed class DistanceBasedFarePolicy : FarePolicy
{
    public decimal RatePerKm { get; private set; }
    
    private DistanceBasedFarePolicy() : base() { }

    public DistanceBasedFarePolicy(Guid id, string name, decimal ratePerKm)
        : base(id, name, FarePolicyType.DistanceBased)
    {
        if (ratePerKm <= 0)
            throw new ArgumentOutOfRangeException(nameof(ratePerKm), "Rate per km must be greater than zero.");
        
        RatePerKm = ratePerKm;
    }
}