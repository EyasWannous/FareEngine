using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.Modifications.DTOs;

public class ModificationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ModificationType Type { get; set; }
    
    public decimal? Surcharge { get; set; }
    public decimal? DiscountPercentage { get; set; }
    
    public static ModificationDto MapFromModel(Modification input)
    {
        return new ModificationDto
        {
            Id = input.Id,
            Name = input.Name,
            Type = input.Type,
            Surcharge = input is FirstClassModification fcm ? fcm.Surcharge : null,
            DiscountPercentage = input is SeniorDiscountModification sdm ? sdm.DiscountPercentage : null
        };
    }

    public static ModificationDto MapFromViewModel(ModificationViewModel input)
    {
        return new ModificationDto
        {
            Id = input.Id,
            Name = input.Name,
            Type = input.Type,
            Surcharge = input.Surcharge,
            DiscountPercentage = input.DiscountPercentage
        };
    }
}