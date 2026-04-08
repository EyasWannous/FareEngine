using FareEngine.Application.SoldProducts;
using FareEngine.Application.SoldProducts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FareEngine.API.Controllers.SoldProducts;

[ApiController]
[Route("api/sold-products")]
public sealed class SoldProductController(
    ISoldProductAppService soldProductAppService,
    IBillService billService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await soldProductAppService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await soldProductAppService.GetByIdAsync(id, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost("daily-pass")]
    public async Task<IActionResult> CreateDailyPass(
        [FromBody] CreateDailyPassRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await soldProductAppService.CreateDailyPassAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("hybrid-trip")]
    public async Task<IActionResult> CreateHybridTrip(
        [FromBody] CreateHybridTripRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await soldProductAppService.CreateHybridTripAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:guid}/bill")]
    public async Task<IActionResult> GetBill(Guid id, CancellationToken cancellationToken)
    {
        var bill = await billService.CalculateAsync(id, cancellationToken);
        return Ok(bill);
    }
}
