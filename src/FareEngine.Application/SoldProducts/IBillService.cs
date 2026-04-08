using FareEngine.Application.SoldProducts.DTOs;

namespace FareEngine.Application.SoldProducts;

public interface IBillService
{
    Task<BillResultDto> CalculateAsync(Guid soldProductId, CancellationToken cancellationToken = default);
}