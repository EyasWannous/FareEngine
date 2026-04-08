namespace FareEngine.Domain.Modifications;

public class ModificationViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ModificationType Type { get; set; }
    
    public decimal? Surcharge { get; set; }
    public decimal? DiscountPercentage { get; set; }
}
