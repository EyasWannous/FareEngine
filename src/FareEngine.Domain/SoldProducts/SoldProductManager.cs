namespace FareEngine.Domain.SoldProducts;

public sealed class SoldProductManager
{
    public SoldProduct CreateDailyPass(
        Guid farePolicyId,
        List<Guid> modificationIds)
    {
        var product = new SoldDailyPass(Guid.CreateVersion7());

        product.AddFarePolicy(farePolicyId);

        foreach (var modId in modificationIds)
            product.AddModification(modId);

        return product;
    }

    public SoldProduct CreateHybridTrip(
        decimal distanceInKm,
        int zoneNumber,
        Guid distancePolicyId,
        Guid zonePolicyId,
        List<Guid> modificationIds)
    {
        var product = new SoldHybridTrip(Guid.CreateVersion7(), distanceInKm, zoneNumber);

        product.AddFarePolicy(distancePolicyId);
        product.AddFarePolicy(zonePolicyId);

        foreach (var modId in modificationIds)
            product.AddModification(modId);

        return product;
    }
}