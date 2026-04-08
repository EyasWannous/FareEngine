using FareEngine.Domain.Modifications;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.Modifications;

public sealed class ModificationReadRepository(AppDbContext context) : IModificationReadRepository
{
    public async Task<ModificationViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await context.Modifications
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new ModificationViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Type = m.Type,
                Surcharge = m is FirstClassModification ? ((FirstClassModification)m).Surcharge : null,
                DiscountPercentage = m is SeniorDiscountModification ? ((SeniorDiscountModification)m).DiscountPercentage : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        return data;
    }

    public async Task<List<ModificationViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var data = await context.Modifications
            .AsNoTracking()
            .Select(m => new ModificationViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Type = m.Type,
                Surcharge = m is FirstClassModification ? ((FirstClassModification)m).Surcharge : null,
                DiscountPercentage = m is SeniorDiscountModification ? ((SeniorDiscountModification)m).DiscountPercentage : null
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}