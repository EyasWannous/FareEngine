using FareEngine.Application.FarePolicies.DTOs;
using FareEngine.Application.Modifications.DTOs;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.SoldProducts.DTOs;

public class SoldProductDto
{
    public Guid Id { get; set; }
    public ProductType Type { get; set; }

    public List<FarePolicyDto> FarePolicies { get; set; } = [];
    public List<ModificationDto> Modifications { get; set; } = [];

    public static SoldProductDto MapFromViewModel(SoldProductViewModel input)
    {
        return new SoldProductDto
        {
            Id = input.Id,
            Type = input.Type,
            FarePolicies = input.FarePolicies.Select(FarePolicyDto.MapFromViewModel).ToList(),
            Modifications = input.Modifications.Select(ModificationDto.MapFromViewModel).ToList()
        };
    }
}