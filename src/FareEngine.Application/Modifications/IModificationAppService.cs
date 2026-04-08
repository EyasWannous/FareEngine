using FareEngine.Application.Modifications.DTOs;
using FareEngine.Domain.Modifications;

namespace FareEngine.Application.Modifications;

public interface IModificationAppService
{
    Task<ModificationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ModificationDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateFirstClassAsync(CreateFirstClassRequestDto input, CancellationToken cancellationToken = default);
    Task<Guid> CreateSeniorDiscountAsync(CreateSeniorDiscountRequestDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}