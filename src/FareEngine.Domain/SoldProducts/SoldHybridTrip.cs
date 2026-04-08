namespace FareEngine.Domain.SoldProducts;

public sealed class SoldHybridTrip : SoldProduct
{
    public decimal DistanceInKm { get; private set; }
    public int ZoneNumber { get; private set; }
    
    private SoldHybridTrip() : base() { }
    
    internal SoldHybridTrip(Guid id, decimal distanceInKm, int zoneNumber) : base(id, ProductType.Hybrid)
    {
        if (distanceInKm <= 0)
            throw new ArgumentOutOfRangeException(nameof(distanceInKm), "Distance in km must be greater than zero.");
        
        if (zoneNumber <= 0)
            throw new ArgumentOutOfRangeException(nameof(zoneNumber), "Zone number must be greater than zero.");
 
        DistanceInKm = distanceInKm;
        ZoneNumber = zoneNumber;
    }
}