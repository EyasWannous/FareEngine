namespace FareEngine.Domain.FarePolicies;

public interface IFarePolicyRepository
{
    Task<FarePolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FarePolicy> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FarePolicy>> GetListByIdsAsync(List<Guid> farePolicyIds, CancellationToken cancellationToken = default);
    Task AddAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default);
    Task DeleteAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default);
    
    Task ThrowIfNotExistsAsync(IEnumerable<FarePolicyIdWithType> farePolicyIdWithTypes, CancellationToken cancellationToken = default);
}