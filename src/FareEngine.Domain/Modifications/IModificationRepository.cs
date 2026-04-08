namespace FareEngine.Domain.Modifications;

public interface IModificationRepository
{
    Task<Modification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Modification> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Modification>> GetListByIdsAsync(List<Guid> modificationIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Modification>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Modification modification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Modification modification, CancellationToken cancellationToken = default);
    Task DeleteAsync(Modification modification, CancellationToken cancellationToken = default);
}