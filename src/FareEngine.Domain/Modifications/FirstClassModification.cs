namespace FareEngine.Domain.Modifications;

public sealed class FirstClassModification : Modification
{
    public decimal Surcharge { get; private set; }
    
    private FirstClassModification() : base() { }

    public FirstClassModification(Guid id, string name, decimal surcharge)
        : base(id, name, ModificationType.FirstClass)
    {
        if (surcharge <= 0)
            throw new ArgumentOutOfRangeException(nameof(surcharge), "Surcharge must be greater than zero.");
        
        Surcharge = surcharge;
    }
}