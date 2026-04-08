namespace FareEngine.Application.SoldProducts.DTOs;

public record BillResultDto(
    Guid SoldProductId,
    decimal BaseFare,
    List<FareLineDto> FareLines,
    List<ModificationLineDto> ModificationLines,
    decimal FinalFare
);