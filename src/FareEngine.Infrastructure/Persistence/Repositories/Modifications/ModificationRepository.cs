using FareEngine.Domain.Modifications;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.Modifications;

public sealed class ModificationRepository : IModificationRepository
{
    private readonly AppDbContext _dbContext;

    public ModificationRepository(AppDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<Modification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Modifications
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Modification> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Modification '{id}' was not found.");

    public async Task<IReadOnlyCollection<Modification>> GetListByIdsAsync(List<Guid> modificationIds, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Modifications
            .AsNoTracking()
            .Where(x => modificationIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Modification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var modifications = await _dbContext.Modifications
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return modifications;
    }

    public async Task AddAsync(Modification modification, CancellationToken cancellationToken = default)
    {
        await _dbContext.Modifications.AddAsync(modification, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Modification modification, CancellationToken cancellationToken = default)
    {
        _dbContext.Modifications.Update(modification);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Modification modification, CancellationToken cancellationToken = default)
    {
        _dbContext.Modifications.Remove(modification);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}