using FareEngine.Domain.Modifications;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.Modifications;

public sealed class ModificationRepository(AppDbContext dbContext) : IModificationRepository
{
    public async Task<Modification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Modifications
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Modification> GetOrThrowByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await GetByIdAsync(id, cancellationToken)
           ?? throw new InvalidOperationException($"Modification '{id}' was not found.");

    public async Task<IReadOnlyCollection<Modification>> GetListByIdsAsync(List<Guid> modificationIds, CancellationToken cancellationToken = default)
    {
        return await dbContext.Modifications
            .AsNoTracking()
            .Where(x => modificationIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Modification modification, CancellationToken cancellationToken = default)
    {
        await dbContext.Modifications.AddAsync(modification, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Modification modification, CancellationToken cancellationToken = default)
    {
        dbContext.Modifications.Remove(modification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ThrowIfNotExistsAsync(IEnumerable<Guid> modificationIds, CancellationToken cancellationToken = default)
    {
        var existingModificationIds = await dbContext.Modifications
            .AsNoTracking()
            .Where(m => modificationIds.Contains(m.Id))
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);

        var missingIds = modificationIds.Except(existingModificationIds).ToList();
        if (missingIds.Count > 0)
            throw new InvalidOperationException($"Modifications {string.Join(", ", missingIds)} were not found.");
    }
}