namespace FareEngine.Domain.SoldProducts;

public interface ISoldProductRepository
{
    Task<SoldProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SoldProduct> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SoldProduct>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(SoldProduct soldProduct, CancellationToken cancellationToken = default);

    Task<bool> AnyByModificationIdAsync(Guid modificationId, CancellationToken cancellationToken = default);
    Task<bool> AnyByFarePolicyIdAsync(Guid farePolicyId, CancellationToken cancellationToken = default);
}