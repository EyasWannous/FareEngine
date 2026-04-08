using FareEngine.Application.FarePolicies.DTOs;
using FareEngine.Domain.FarePolicies;

namespace FareEngine.Application.FarePolicies;

public interface IFarePolicyAppService
{
    Task<FarePolicyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FarePolicyDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateFlatRateAsync(CreateFlatRateRequestDto input, CancellationToken cancellationToken = default);
    Task<Guid> CreateDistanceBasedAsync(CreateDistanceBasedRequestDto input, CancellationToken cancellationToken = default);
    Task<Guid> CreateZoneBasedAsync(CreateZoneBasedRequestDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}