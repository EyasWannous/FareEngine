using FareEngine.Application.Modifications.DTOs;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;

namespace FareEngine.Application.Modifications;

public sealed class ModificationAppService(IModificationRepository modificationRepository, ISoldProductRepository soldProductRepository, IModificationReadRepository modificationReadRepository) : IModificationAppService
{
    public async Task<ModificationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var modification = await modificationReadRepository.GetByIdAsync(id, cancellationToken);
        if (modification is null)
            return null;
        
        return ModificationDto.MapFromViewModel(modification);
    }

    public async Task<IEnumerable<ModificationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var modifications = await modificationReadRepository.GetAllAsync(cancellationToken);

        return modifications.Select(ModificationDto.MapFromViewModel).ToList();
    }

    public async Task<Guid> CreateFirstClassAsync(CreateFirstClassRequestDto input, CancellationToken cancellationToken = default)
    {
        var modification = new FirstClassModification(Guid.CreateVersion7(), input.Name, input.Surcharge);
        await modificationRepository.AddAsync(modification, cancellationToken);
        return modification.Id;
    }

    public async Task<Guid> CreateSeniorDiscountAsync(CreateSeniorDiscountRequestDto input, CancellationToken cancellationToken = default)
    {
        var modification = new SeniorDiscountModification(Guid.CreateVersion7(), input.Name, input.DiscountPercentage);
        await modificationRepository.AddAsync(modification, cancellationToken);
        return modification.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var modification = await modificationRepository.GetOrThrowByIdAsync(id, cancellationToken);
        
        if (await soldProductRepository.AnyByModificationIdAsync(id, cancellationToken))
            throw new InvalidOperationException("Fare policy cannot be deleted as it is associated with sold products.");

        await modificationRepository.DeleteAsync(modification, cancellationToken);
    }
}