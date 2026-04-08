namespace FareEngine.Domain.FarePolicies;

public interface IFarePolicyCalculatorFactory
{
    IFarePolicyCalculator Create(FarePolicyType farePolicyType);
}