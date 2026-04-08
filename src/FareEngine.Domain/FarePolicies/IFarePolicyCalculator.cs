using FareEngine.Domain.SoldProducts;

namespace FareEngine.Domain.FarePolicies;

public interface IFarePolicyCalculator
{
    FareCalculationResult Calculate(FarePolicy farePolicy, SoldProduct soldProduct);
}