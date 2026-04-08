using FareEngine.Domain.Modifications;

namespace FareEngine.Infrastructure.Modifications;

public sealed class ModificationCalculatorFactory : IModificationCalculatorFactory
{
    private readonly FirstClassModificationCalculator _firstClass;
    private readonly SeniorDiscountModificationCalculator _seniorDiscount;

    public ModificationCalculatorFactory(
        FirstClassModificationCalculator firstClass,
        SeniorDiscountModificationCalculator seniorDiscount)
    {
        _firstClass = firstClass;
        _seniorDiscount = seniorDiscount;
    }
    
    public IModificationCalculator Create(ModificationType modificationType)
    {
        return modificationType switch
        {
            ModificationType.FirstClass => _firstClass,
            ModificationType.SeniorDiscount => _seniorDiscount,
            _ => throw new ArgumentOutOfRangeException(nameof(modificationType), modificationType, null)
        };
    }
}