namespace FareEngine.Domain.SoldProducts;

public sealed class SoldProductModification
{
    public Guid SoldProductId { get; private set; }
    public Guid ModificationId { get; private set; }
    
    private SoldProductModification() { }

    internal SoldProductModification(Guid soldProductId, Guid modificationId)
    {
        SoldProductId = soldProductId;
        ModificationId = modificationId;
    }
}