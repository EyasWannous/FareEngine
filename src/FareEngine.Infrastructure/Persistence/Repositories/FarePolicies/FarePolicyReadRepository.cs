using FareEngine.Domain.FarePolicies;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.FarePolicies;

public sealed class FarePolicyReadRepository(AppDbContext context) : IFarePolicyReadRepository
{
    public async Task<FarePolicyViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await context.FarePolicies
            .AsNoTracking()
            .Where(fp => fp.Id == id)
            .Select(fp => new FarePolicyViewModel
            {
                Id = fp.Id,
                Name = fp.Name,
                Type = fp.Type,
                RatePerKm = fp is DistanceBasedFarePolicy ? ((DistanceBasedFarePolicy)fp).RatePerKm : null,
                FlatAmount = fp is FlatRateFarePolicy ? ((FlatRateFarePolicy)fp).FlatAmount : null,
                ZoneNumber = fp is ZoneBasedFarePolicy ? ((ZoneBasedFarePolicy)fp).ZoneNumber : null,
                ZonePrice = fp is ZoneBasedFarePolicy ? ((ZoneBasedFarePolicy)fp).ZonePrice : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        return data;
    }

    public async Task<List<FarePolicyViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var data = await context.FarePolicies
            .AsNoTracking()
            .Select(fp => new FarePolicyViewModel
            {
                Id = fp.Id,
                Name = fp.Name,
                Type = fp.Type,
                RatePerKm = fp is DistanceBasedFarePolicy ? ((DistanceBasedFarePolicy)fp).RatePerKm : null,
                FlatAmount = fp is FlatRateFarePolicy ? ((FlatRateFarePolicy)fp).FlatAmount : null,
                ZoneNumber = fp is ZoneBasedFarePolicy ? ((ZoneBasedFarePolicy)fp).ZoneNumber : null,
                ZonePrice = fp is ZoneBasedFarePolicy ? ((ZoneBasedFarePolicy)fp).ZonePrice : null
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}