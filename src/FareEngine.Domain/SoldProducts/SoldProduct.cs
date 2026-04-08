namespace FareEngine.Domain.SoldProducts;

public abstract class SoldProduct
{
    public Guid Id { get; private set; }
    public ProductType Type { get; private set; }
    
    public IReadOnlyCollection<SoldProductFarePolicy> FarePolicies => _farePolicies.AsReadOnly();
    public IReadOnlyCollection<SoldProductModification> Modifications => _modifications.AsReadOnly();

    private readonly List<SoldProductFarePolicy> _farePolicies = [];
    private readonly List<SoldProductModification> _modifications = [];
    
    protected SoldProduct() { }
    
    protected SoldProduct(Guid id, ProductType type)
    {
        Id = id;
        Type = type;
    }
    
    internal void AddFarePolicy(Guid farePolicyId)
        => _farePolicies.Add(new SoldProductFarePolicy(Id, farePolicyId));

    internal void AddModification(Guid modificationId)
        => _modifications.Add(new SoldProductModification(Id, modificationId));
    
    internal void AddFarePolicies(List<Guid> farePolicyIds)
        => _farePolicies.AddRange(farePolicyIds.Select(farePolicyId => new SoldProductFarePolicy(Id, farePolicyId)));
    
    internal void AddModifications(List<Guid> modificationsIds)
        => _modifications.AddRange(modificationsIds.Select(modificationId => new SoldProductModification(Id, modificationId)));

}