namespace FareEngine.Application.SoldProducts.DTOs;

public sealed record CreateDailyPassRequestDto(
    Guid FarePolicyId,
    List<Guid> ModificationIds);