namespace FareEngine.Domain.FarePolicies;

public sealed class FlatRateFarePolicy : FarePolicy
{
    public decimal FlatAmount { get; private set; }
    
    private FlatRateFarePolicy() : base() { }

    public FlatRateFarePolicy(Guid id, string name, decimal flatAmount)
        : base(id, name, FarePolicyType.FlatRate)
    {
        if (flatAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(flatAmount), "Flat amount must be greater than zero.");
        
        FlatAmount = flatAmount;
    }
}