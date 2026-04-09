using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;

namespace FareEngine.Domain.SoldProducts;

public sealed class SoldProductManager(IFarePolicyRepository farePolicyRepository, IModificationRepository modificationRepository)
{
    public async Task<SoldProduct> CreateDailyPassAsync(
        Guid farePolicyId,
        List<Guid> modificationIds,
        CancellationToken cancellationToken = default)
    {
        await farePolicyRepository.ThrowIfNotExistsAsync(
            [new FarePolicyIdWithType(farePolicyId, FarePolicyType.FlatRate)], 
            cancellationToken
        );

        var modificationSetIds = modificationIds.ToHashSet();
        
        await modificationRepository.ThrowIfNotExistsAsync(modificationSetIds, cancellationToken);
        
        var product = new SoldDailyPass(Guid.CreateVersion7());

        product.AddFarePolicy(farePolicyId);

        product.AddModifications(modificationSetIds);

        return product;
    }

    public async Task<SoldProduct> CreateHybridTripAsync(
        decimal distanceInKm,
        Guid distancePolicyId,
        Guid zonePolicyId,
        List<Guid> modificationIds,
        CancellationToken cancellationToken = default)
    {
        var zonePolicy = await farePolicyRepository.GetByIdAsync(zonePolicyId, cancellationToken);
        if (zonePolicy is not ZoneBasedFarePolicy zoneBasedPolicy)
            throw new InvalidOperationException("Hybrid trip requires a zone based fare policy.");

        await farePolicyRepository.ThrowIfNotExistsAsync([
            new FarePolicyIdWithType(distancePolicyId, FarePolicyType.DistanceBased),
            new FarePolicyIdWithType(zonePolicyId, FarePolicyType.ZoneBased)],
            cancellationToken
        );
        
        var modificationSetIds = modificationIds.ToHashSet();
        
        await modificationRepository.ThrowIfNotExistsAsync(modificationSetIds, cancellationToken);
        
        var product = new SoldHybridTrip(Guid.CreateVersion7(), distanceInKm, zoneBasedPolicy.ZoneNumber);

        product.AddFarePolicies([distancePolicyId, zonePolicyId]);
        product.AddModifications(modificationSetIds);

        return product;
    }
}