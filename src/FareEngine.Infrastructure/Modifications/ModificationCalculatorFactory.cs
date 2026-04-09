using FareEngine.Domain.Modifications;

namespace FareEngine.Infrastructure.Modifications;

public sealed class ModificationCalculatorFactory(
    FirstClassModificationCalculator firstClass,
    SeniorDiscountModificationCalculator seniorDiscount)
    : IModificationCalculatorFactory
{
    public IModificationCalculator Create(ModificationType modificationType)
    {
        return modificationType switch
        {
            ModificationType.FirstClass => firstClass,
            ModificationType.SeniorDiscount => seniorDiscount,
            _ => throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null)
        };
    }
}