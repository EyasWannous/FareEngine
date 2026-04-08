namespace FareEngine.Domain.FarePolicies;

public interface IFarePolicyReadRepository
{
    Task<FarePolicyViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<FarePolicyViewModel>> GetAllAsync(CancellationToken cancellationToken = default);
}