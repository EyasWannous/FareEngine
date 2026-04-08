using FareEngine.Application.SoldProducts.DTOs;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.SoldProducts;

public interface ISoldProductAppService
{
    Task<Guid> CreateDailyPassAsync(
        CreateDailyPassRequestDto input,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateHybridTripAsync(
        CreateHybridTripRequestDto input,
        CancellationToken cancellationToken = default);
    
    Task<SoldProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SoldProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
}