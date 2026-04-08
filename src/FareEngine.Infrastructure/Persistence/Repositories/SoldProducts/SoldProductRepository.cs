using FareEngine.Domain.SoldProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FareEngine.Infrastructure.Persistence.Repositories.SoldProducts;

public sealed class SoldProductRepository : ISoldProductRepository
{
    private readonly AppDbContext _dbContext;

    public SoldProductRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<SoldProduct?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SoldProducts
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<SoldProduct> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Sold product '{id}' was not found.");

    public async Task<IReadOnlyCollection<SoldProduct>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var soldProducts = await _dbContext.SoldProducts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return soldProducts;
    }

    public async Task AddAsync(SoldProduct soldProduct, CancellationToken cancellationToken = default)
    {
        await _dbContext.SoldProducts.AddAsync(soldProduct, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SoldProduct soldProduct, CancellationToken cancellationToken = default)
    {
        _dbContext.SoldProducts.Update(soldProduct);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(SoldProduct soldProduct, CancellationToken cancellationToken = default)
    {
        _dbContext.SoldProducts.Remove(soldProduct);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> AnyByModificationIdAsync(Guid modificationId, CancellationToken cancellationToken = default)
    {
        return _dbContext.SoldProducts.AnyAsync(
            x => x.Modifications.Any(
                y => y.ModificationId == modificationId
            ), 
            cancellationToken: cancellationToken
        );
    }

    public Task<bool> AnyByFarePolicyIdAsync(Guid farePolicyId, CancellationToken cancellationToken = default)
    {
        return _dbContext.SoldProducts.AnyAsync(
            x => x.FarePolicies.Any(
                y => y.FarePolicyId == farePolicyId
            ), 
            cancellationToken: cancellationToken
        );
    }


}