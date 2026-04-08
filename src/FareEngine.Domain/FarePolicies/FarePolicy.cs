namespace FareEngine.Domain.FarePolicies;

public abstract class FarePolicy
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public FarePolicyType Type { get; private set; }
    
    protected FarePolicy() { }
    
    protected FarePolicy(Guid id, string name, FarePolicyType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}