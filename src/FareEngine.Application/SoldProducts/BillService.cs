using FareEngine.Application.SoldProducts.DTOs;
using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.SoldProducts;

public class BillService : IBillService
{
    private readonly ISoldProductRepository _soldProductRepository;
    private readonly IFarePolicyRepository _farePolicyRepository;
    private readonly IModificationRepository _modificationRepository;
    private readonly IFarePolicyCalculatorFactory _farePolicyCalculatorFactory;
    private readonly IModificationCalculatorFactory _modificationCalculatorFactory;

    public BillService(
        ISoldProductRepository soldProductRepository,
        IFarePolicyRepository farePolicyRepository,
        IModificationRepository modificationRepository,
        IFarePolicyCalculatorFactory farePolicyCalculatorFactory,
        IModificationCalculatorFactory modificationCalculatorFactory)
    {
        _soldProductRepository = soldProductRepository;
        _farePolicyRepository = farePolicyRepository;
        _modificationRepository = modificationRepository;
        _farePolicyCalculatorFactory = farePolicyCalculatorFactory;
        _modificationCalculatorFactory = modificationCalculatorFactory;
    }

    public async Task<BillResultDto> CalculateAsync(Guid soldProductId, CancellationToken cancellationToken = default)
    {
        var soldProduct = await _soldProductRepository.GetOrThrowByIdAsync(soldProductId, cancellationToken);

        (var fareLines, decimal baseFare) = await CalculateFarePoliciesAsync(soldProduct);

        (var modificationLines, decimal currentFare) = await CalculateModificationsAsync(baseFare, soldProduct);

        decimal finalFare = Math.Max(currentFare, 0);

        return new BillResultDto(
            SoldProductId: soldProductId,
            BaseFare: baseFare,
            FareLines: fareLines,
            ModificationLines: modificationLines,
            FinalFare: finalFare
        );
    }

    private async Task<(List<ModificationLineDto> modificationLines, decimal currentFare)> CalculateModificationsAsync(decimal baseFare, SoldProduct soldProduct)
    {
        var modificationLines = new List<ModificationLineDto>();
        decimal currentFare = baseFare;

        var modificationIds = soldProduct.Modifications.Select(x => x.ModificationId).ToList();
        var modifications = await _modificationRepository.GetListByIdsAsync(modificationIds);

        foreach (var modification in modifications)
        {
            var calculator = _modificationCalculatorFactory.Create(modification.Type);
            var result = calculator.Calculate(modification, currentFare);

            currentFare += result.Delta;
            modificationLines.Add(new ModificationLineDto(result.Label, result.Delta));
        }

        return (modificationLines, currentFare);
    }

    private async Task<(List<FareLineDto> fareLines, decimal baseFare)> CalculateFarePoliciesAsync(SoldProduct soldProduct)
    {
        var fareLines = new List<FareLineDto>();
        decimal baseFare = 0;

        var farePolicyIds = soldProduct.FarePolicies.Select(x => x.FarePolicyId).ToList();
        var farePolicies = await _farePolicyRepository.GetListByIdsAsync(farePolicyIds);
        
        foreach (var farePolicy in farePolicies)
        {
            var calculator = _farePolicyCalculatorFactory.Create(farePolicy.Type);
            var result = calculator.Calculate(farePolicy, soldProduct);

            baseFare += result.Amount;
            fareLines.Add(new FareLineDto(result.Label, result.Amount));
        }

        return (fareLines, baseFare);
    }
}