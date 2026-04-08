namespace FareEngine.Domain.Modifications;

public interface IModificationCalculator
{
    ModificationResult Calculate(Modification modification, decimal currentFare);
}