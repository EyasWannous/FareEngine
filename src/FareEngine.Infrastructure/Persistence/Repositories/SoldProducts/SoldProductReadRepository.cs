using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence.Repositories.SoldProducts;

public sealed class SoldProductReadRepository(AppDbContext context) : ISoldProductReadRepository
{
    public async Task<SoldProductViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await context.SoldProducts
            .AsNoTracking()
            .Where(sp => sp.Id == id)
            .Select(sp => new
            {
                sp.Id,
                sp.Type,
                FarePolicies = context.FarePolicies
                    .Where(fp => sp.FarePolicies.Select(x => x.FarePolicyId).Contains(fp.Id))
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
                    .ToList(),
                Modifications = context.Modifications
                    .Where(m => sp.Modifications.Select(x => x.ModificationId).Contains(m.Id))
                    .Select(m => new ModificationViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Type = m.Type,
                        Surcharge = m is FirstClassModification ? ((FirstClassModification)m).Surcharge : null,
                        DiscountPercentage = m is SeniorDiscountModification ? ((SeniorDiscountModification)m).DiscountPercentage : null
                    })
                    .ToList()
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        if (data is null)
            return null;

        return new SoldProductViewModel(
            data.Id, 
            data.Type, 
            data.FarePolicies, 
            data.Modifications
        );
    }

    public async Task<IReadOnlyCollection<SoldProductViewModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dataList = await context.SoldProducts
            .AsNoTracking()
            .Select(sp => new
            {
                sp.Id,
                sp.Type,
                FarePolicies = context.FarePolicies
                    .Where(fp => sp.FarePolicies.Select(x => x.FarePolicyId).Contains(fp.Id))
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
                    .ToList(),
                Modifications = context.Modifications
                    .Where(m => sp.Modifications.Select(x => x.ModificationId).Contains(m.Id))
                    .Select(m => new ModificationViewModel
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Type = m.Type,
                        Surcharge = m is FirstClassModification ? ((FirstClassModification)m).Surcharge : null,
                        DiscountPercentage = m is SeniorDiscountModification ? ((SeniorDiscountModification)m).DiscountPercentage : null
                    })
                    .ToList()
            })
            .AsSplitQuery() 
            .ToListAsync(cancellationToken);

        return dataList
            .Select(data => new SoldProductViewModel(
                data.Id,
                data.Type,
                data.FarePolicies,
                data.Modifications)
            ).ToList();
    }
}