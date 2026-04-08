namespace FareEngine.Domain.FarePolicies;

public class FarePolicyViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FarePolicyType Type { get; set; }
    
    public decimal? RatePerKm { get; set; }
    public decimal? FlatAmount { get; set; }
    public int? ZoneNumber { get; set; }
    public decimal? ZonePrice { get; set; }
}
