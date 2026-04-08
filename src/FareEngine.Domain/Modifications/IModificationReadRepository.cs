namespace FareEngine.Domain.Modifications;

public interface IModificationReadRepository
{
    Task<ModificationViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ModificationViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
}