using FareEngine.Application.SoldProducts.DTOs;
using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.SoldProducts;

public sealed class SoldProductAppService : ISoldProductAppService
{
    private readonly SoldProductManager _soldProductManager;
    private readonly ISoldProductRepository _soldProductRepository;
    private readonly IFarePolicyRepository _farePolicyRepository;
    private readonly IModificationRepository _modificationRepository;
    private readonly ISoldProductReadRepository _soldProductReadRepository;

    public SoldProductAppService(
        SoldProductManager soldProductManager,
        ISoldProductRepository soldProductRepository,
        IFarePolicyRepository farePolicyRepository,
        IModificationRepository modificationRepository,
        ISoldProductReadRepository soldProductReadRepository)
    {
        _soldProductManager = soldProductManager;
        _soldProductRepository = soldProductRepository;
        _farePolicyRepository = farePolicyRepository;
        _modificationRepository = modificationRepository;
        _soldProductReadRepository = soldProductReadRepository;
    }

    public async Task<Guid> CreateDailyPassAsync(
        CreateDailyPassRequestDto input,
        CancellationToken cancellationToken = default)
    {
        var policy = await _farePolicyRepository.GetByIdAsync(input.FarePolicyId, cancellationToken);
        if (policy is not FlatRateFarePolicy)
            throw new InvalidOperationException("Daily pass requires a flat rate fare policy.");

        foreach (var modId in input.ModificationIds)
        {
            var mod = await _modificationRepository.GetByIdAsync(modId, cancellationToken);
            if (mod is null)
                throw new InvalidOperationException($"Modification {modId} not found.");
        }

        var product = _soldProductManager.CreateDailyPass(
            input.FarePolicyId,
            input.ModificationIds
        );

        await _soldProductRepository.AddAsync(product, cancellationToken);

        return product.Id;
    }

    public async Task<Guid> CreateHybridTripAsync(
        CreateHybridTripRequestDto input,
        CancellationToken cancellationToken = default)
    {
        var distancePolicy = await _farePolicyRepository.GetByIdAsync(input.DistancePolicyId, cancellationToken);
        if (distancePolicy is not DistanceBasedFarePolicy)
            throw new InvalidOperationException("Hybrid trip requires a distance based fare policy.");

        var zonePolicy = await _farePolicyRepository.GetByIdAsync(input.ZonePolicyId, cancellationToken);
        if (zonePolicy is not ZoneBasedFarePolicy zoneBasedPolicy)
            throw new InvalidOperationException("Hybrid trip requires a zone based fare policy.");

        foreach (var modId in input.ModificationIds)
        {
            var mod = await _modificationRepository.GetByIdAsync(modId, cancellationToken);
            if (mod is null)
                throw new InvalidOperationException($"Modification {modId} not found.");
        }

        var product = _soldProductManager.CreateHybridTrip(
            input.DistanceInKm,
            zoneBasedPolicy.ZoneNumber,
            input.DistancePolicyId,
            input.ZonePolicyId,
            input.ModificationIds
        );

        await _soldProductRepository.AddAsync(product, cancellationToken);

        return product.Id;
    }

    public async Task<SoldProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var soldProductViewModel = await _soldProductReadRepository.GetByIdAsync(id, cancellationToken);
        if (soldProductViewModel is null)
            return null;
         
        return SoldProductDto.MapFromViewModel(soldProductViewModel);
   }

    public async Task<IEnumerable<SoldProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var soldProductViewModels = await _soldProductReadRepository.GetAllAsync(cancellationToken);
        
        return soldProductViewModels.Select(SoldProductDto.MapFromViewModel).ToList();
    }
}