namespace FareEngine.Domain.Modifications;

public abstract class Modification
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ModificationType Type { get; private set; }
    
    protected Modification() { }

    protected Modification(Guid id, string name, ModificationType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}