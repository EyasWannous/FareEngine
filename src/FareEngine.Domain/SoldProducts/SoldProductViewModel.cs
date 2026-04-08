using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;

namespace FareEngine.Domain.SoldProducts;

public sealed class SoldProductViewModel
{
    public Guid Id { get; private set; }
    public ProductType Type { get; private set; }
    
    public IReadOnlyList<FarePolicyViewModel> FarePolicies { get; }
    public IReadOnlyList<ModificationViewModel> Modifications { get; }

    public SoldProductViewModel(
        Guid id,
        ProductType type,
        IReadOnlyList<FarePolicyViewModel> farePolicies,
        IReadOnlyList<ModificationViewModel> modifications)
    {
        Id = id;
        Type = type;
        FarePolicies = farePolicies;
        Modifications = modifications;
    }
}