namespace FareEngine.Application.SoldProducts.DTOs;

public sealed record CreateHybridTripRequestDto(
    decimal DistanceInKm,
    Guid DistancePolicyId,
    Guid ZonePolicyId,
    List<Guid> ModificationIds);