using FareEngine.Domain.FarePolicies;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.FarePolicies;

public sealed class FarePolicyRepository(AppDbContext dbContext) : IFarePolicyRepository
{
    public async Task<FarePolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FarePolicies
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<FarePolicy> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Fare policy '{id}' was not found.");

    public async Task<IReadOnlyCollection<FarePolicy>> GetListByIdsAsync(List<Guid> farePolicyIds, CancellationToken cancellationToken = default)
    {
        return await dbContext.FarePolicies
            .AsNoTracking()
            .Where(x => farePolicyIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default)
    {
        await dbContext.FarePolicies.AddAsync(farePolicy, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async Task DeleteAsync(FarePolicy farePolicy, CancellationToken cancellationToken = default)
    {
        dbContext.FarePolicies.Remove(farePolicy);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ThrowIfNotExistsAsync(IEnumerable<FarePolicyIdWithType> farePolicyIdWithTypes, CancellationToken cancellationToken = default)
    {
        var farePolicyIdWithTypeList = farePolicyIdWithTypes.ToList();
        
        var existingFarePolicyIdWithTypeList = await dbContext.FarePolicies
            .AsNoTracking()
            .Where(fp => farePolicyIdWithTypeList.Select(x => x.FarePolicyId).Contains(fp.Id))
            .Select(fp => new FarePolicyIdWithType(fp.Id, fp.Type))
            .ToListAsync(cancellationToken);

        var missing = new List<FarePolicyIdWithType>();
        foreach (var existing in existingFarePolicyIdWithTypeList)
        {
            var farePolicyIdWithType = farePolicyIdWithTypeList.FirstOrDefault(
                x => x.FarePolicyId == existing.FarePolicyId && 
                x.Type == existing.Type
            );
            if (farePolicyIdWithType is null)
                missing.Add(existing);
        }

        if (missing.Count > 0)
            throw new InvalidOperationException($"Fare policies: {string.Join(", ", missing.ToString())} were not found.");
    }
}