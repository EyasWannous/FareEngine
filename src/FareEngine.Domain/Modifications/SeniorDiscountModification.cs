namespace FareEngine.Domain.Modifications;

public sealed class SeniorDiscountModification : Modification
{
    public decimal DiscountPercentage { get; private set; }
    
    private SeniorDiscountModification() : base() { }
    
    public SeniorDiscountModification(Guid id, string name, decimal discountPercentage)
        : base(id, name, ModificationType.SeniorDiscount)
    {
        if (discountPercentage is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(discountPercentage), discountPercentage, "Discount percentage must be between 0 and 1.");
        
        DiscountPercentage = discountPercentage;
    }
}