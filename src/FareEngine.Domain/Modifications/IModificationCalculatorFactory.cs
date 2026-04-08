namespace FareEngine.Domain.Modifications;

public interface IModificationCalculatorFactory
{
    IModificationCalculator Create(ModificationType modificationType);
}