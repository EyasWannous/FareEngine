using FareEngine.Domain.SoldProducts;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.SoldProducts;

public sealed class SoldProductRepository(AppDbContext dbContext) : ISoldProductRepository
{
    public async Task<SoldProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.SoldProducts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<SoldProduct> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Sold product '{id}' was not found.");

    public async Task<IReadOnlyCollection<SoldProduct>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var soldProducts = await dbContext.SoldProducts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return soldProducts;
    }

    public async Task AddAsync(SoldProduct soldProduct, CancellationToken cancellationToken = default)
    {
        await dbContext.SoldProducts.AddAsync(soldProduct, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> AnyByModificationIdAsync(Guid modificationId, CancellationToken cancellationToken = default)
    {
        return dbContext.SoldProducts.AnyAsync(
            x => x.Modifications.Any(
                y => y.ModificationId == modificationId
            ), 
            cancellationToken: cancellationToken
        );
    }

    public Task<bool> AnyByFarePolicyIdAsync(Guid farePolicyId, CancellationToken cancellationToken = default)
    {
        return dbContext.SoldProducts.AnyAsync(
            x => x.FarePolicies.Any(
                y => y.FarePolicyId == farePolicyId
            ), 
            cancellationToken: cancellationToken
        );
    }


}