namespace FareEngine.Domain.SoldProducts;

public sealed class SoldProductFarePolicy
{
    public Guid SoldProductId { get; private set; }
    public Guid FarePolicyId { get; private set; }
    
    private SoldProductFarePolicy() { }

    internal SoldProductFarePolicy(Guid soldProductId, Guid farePolicyId)
    {
        SoldProductId = soldProductId;
        FarePolicyId = farePolicyId;
    }
}