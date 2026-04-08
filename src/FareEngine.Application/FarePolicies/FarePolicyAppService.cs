using FareEngine.Application.FarePolicies.DTOs;
using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.FarePolicies;

public sealed class FarePolicyAppService(IFarePolicyRepository farePolicyRepository, ISoldProductRepository soldProductRepository, IFarePolicyReadRepository farePolicyReadRepository) : IFarePolicyAppService
{
    public async Task<FarePolicyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var policy = await farePolicyReadRepository.GetByIdAsync(id, cancellationToken);
        if (policy is null)
            return null;
        
        return FarePolicyDto.MapFromViewModel(policy);
    }

    public async Task<IEnumerable<FarePolicyDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var policies = await farePolicyReadRepository.GetAllAsync(cancellationToken);

        return policies.Select(FarePolicyDto.MapFromViewModel).ToList();
    }   

    public async Task<Guid> CreateFlatRateAsync(CreateFlatRateRequestDto input, CancellationToken cancellationToken = default)
    {
        var policy = new FlatRateFarePolicy(Guid.CreateVersion7(), input.Name, input.FlatAmount);
        
        await farePolicyRepository.AddAsync(policy, cancellationToken);
        
        return policy.Id;
    }

    public async Task<Guid> CreateDistanceBasedAsync(CreateDistanceBasedRequestDto input, CancellationToken cancellationToken = default)
    {
        var policy = new DistanceBasedFarePolicy(Guid.CreateVersion7(), input.Name, input.RatePerKm);
        await farePolicyRepository.AddAsync(policy, cancellationToken);
        return policy.Id;
    }

    public async Task<Guid> CreateZoneBasedAsync(CreateZoneBasedRequestDto input, CancellationToken cancellationToken = default)
    {
        var policy = new ZoneBasedFarePolicy(Guid.CreateVersion7(), input.Name, input.ZoneNumber, input.ZonePrice);
        await farePolicyRepository.AddAsync(policy, cancellationToken);
        
        return policy.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var policy = await farePolicyRepository.GetOrThrowByIdAsync(id, cancellationToken);

        if (await soldProductRepository.AnyByFarePolicyIdAsync(id, cancellationToken))
            throw new InvalidOperationException("Fare policy cannot be deleted as it is associated with sold products.");
        
        await farePolicyRepository.DeleteAsync(policy, cancellationToken);
    }
}