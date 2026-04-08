namespace FareEngine.Domain.FarePolicies;

public interface IFarePolicyRepository
{
    Task<FarePolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FarePolicy> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FarePolicy>> GetListByIdsAsync(List<Guid> farePolicyIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FarePolicy>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default);
    Task UpdateAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default);
    Task DeleteAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default);
}