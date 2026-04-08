namespace FareEngine.Domain.FarePolicies;

public sealed class ZoneBasedFarePolicy : FarePolicy
{
    public int ZoneNumber { get; private set; }
    public decimal ZonePrice { get; private set; }
    
    private ZoneBasedFarePolicy() : base() { }
    
    public ZoneBasedFarePolicy(Guid id, string name, int zoneNumber, decimal zonePrice)
        : base(id, name, FarePolicyType.ZoneBased)
    {
        if (zoneNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(zoneNumber), "Zone number must be greater than zero.");
        
        if (zonePrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(zonePrice), "Zone price must be greater than zero.");
 
        ZoneNumber = zoneNumber;
        ZonePrice = zonePrice;
    }
}