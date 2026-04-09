using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.FarePolicies.DTOs;

public class FarePolicyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public FarePolicyType Type { get; set; }
    
    public decimal? RatePerKm { get; set; }
    public decimal? FlatAmount { get; set; }
    public int? ZoneNumber { get; set; }
    public decimal? ZonePrice { get; set; }

    public string FarePolicyTypeString => Type.ToString();
    
    public static FarePolicyDto MapFromModel(FarePolicy input)
    {
        return new FarePolicyDto
        {
            Id = input.Id,
            Name = input.Name,
            Type = input.Type,
            RatePerKm = input is DistanceBasedFarePolicy dbfp ? dbfp.RatePerKm : null,
            FlatAmount = input is FlatRateFarePolicy frfp ? frfp.FlatAmount : null,
            ZoneNumber = input is ZoneBasedFarePolicy zbfp ? zbfp.ZoneNumber : null,
            ZonePrice = input is ZoneBasedFarePolicy zbfp2 ? zbfp2.ZonePrice : null
        };
    }

    public static FarePolicyDto MapFromViewModel(FarePolicyViewModel input)
    {
        return new FarePolicyDto
        {
            Id = input.Id,
            Name = input.Name,
            Type = input.Type,
            RatePerKm = input.RatePerKm,
            FlatAmount = input.FlatAmount,
            ZoneNumber = input.ZoneNumber,
            ZonePrice = input.ZonePrice
        };
    }
}