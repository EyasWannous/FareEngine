namespace FareEngine.Domain.SoldProducts;

public interface ISoldProductReadRepository
{
    Task<SoldProductViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SoldProductViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
}