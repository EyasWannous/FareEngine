namespace FareEngine.Application.FarePolicies.DTOs;

public sealed record CreateZoneBasedRequestDto(string Name, int ZoneNumber, decimal ZonePrice);