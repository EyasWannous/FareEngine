using FareEngine.Application.SoldProducts.DTOs;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.SoldProducts;

public sealed class SoldProductAppService(
    SoldProductManager soldProductManager,
    ISoldProductRepository soldProductRepository,
    ISoldProductReadRepository soldProductReadRepository)
    : ISoldProductAppService
{
    public async Task<Guid> CreateDailyPassAsync(
        CreateDailyPassRequestDto input,
        CancellationToken cancellationToken = default)
    {
        var product = await soldProductManager.CreateDailyPassAsync(
            input.FarePolicyId,
            input.ModificationIds,
            cancellationToken
        );

        await soldProductRepository.AddAsync(product, cancellationToken);

        return product.Id;
    }

    public async Task<Guid> CreateHybridTripAsync(
        CreateHybridTripRequestDto input,
        CancellationToken cancellationToken = default)
    {
        var product = await soldProductManager.CreateHybridTripAsync(
            input.DistanceInKm,
            input.DistancePolicyId,
            input.ZonePolicyId,
            input.ModificationIds,
            cancellationToken
        );

        await soldProductRepository.AddAsync(product, cancellationToken);

        return product.Id;
    }

    public async Task<SoldProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var soldProductViewModel = await soldProductReadRepository.GetByIdAsync(id, cancellationToken);
        if (soldProductViewModel is null)
            return null;
         
        return SoldProductDto.MapFromViewModel(soldProductViewModel);
   }

    public async Task<IEnumerable<SoldProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var soldProductViewModels = await soldProductReadRepository.GetAllAsync(cancellationToken);
        
        return soldProductViewModels.Select(SoldProductDto.MapFromViewModel).ToList();
    }
}