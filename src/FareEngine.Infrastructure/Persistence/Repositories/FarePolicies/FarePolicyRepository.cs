using FareEngine.Domain.FarePolicies;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.FarePolicies;

public sealed class FarePolicyRepository : IFarePolicyRepository
{
    private readonly AppDbContext _dbContext;

    public FarePolicyRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<FarePolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FarePolicies
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<FarePolicy> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Fare policy '{id}' was not found.");

    public async Task<IReadOnlyCollection<FarePolicy>> GetListByIdsAsync(List<Guid> farePolicyIds, CancellationToken cancellationToken = default)
    {
        return await _dbContext.FarePolicies
            .AsNoTracking()
            .Where(x => farePolicyIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<FarePolicy>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var farePolicies = await _dbContext.FarePolicies
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return farePolicies;
    }

    public async Task AddAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default)
    {
        await _dbContext.FarePolicies.AddAsync(farePolicy, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default)
    {
        _dbContext.FarePolicies.Update(farePolicy);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default)
    {
        _dbContext.FarePolicies.Remove(farePolicy);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}