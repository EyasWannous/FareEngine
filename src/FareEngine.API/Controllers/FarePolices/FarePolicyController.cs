using FareEngine.Application.FarePolicies;
using FareEngine.Application.FarePolicies.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FareEngine.API.Controllers.FarePolices;

[ApiController]
[Route("api/fare-policies")]
public sealed class FarePolicyController(IFarePolicyAppService farePolicyAppService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var policies = await farePolicyAppService.GetAllAsync(cancellationToken);
        return Ok(policies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var policy = await farePolicyAppService.GetByIdAsync(id, cancellationToken);
        return policy is null ? NotFound() : Ok(policy);
    }

    [HttpPost("flat-rate")]
    public async Task<IActionResult> CreateFlatRate(
        [FromBody] CreateFlatRateRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await farePolicyAppService.CreateFlatRateAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("distance-based")]
    public async Task<IActionResult> CreateDistanceBased(
        [FromBody] CreateDistanceBasedRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await farePolicyAppService.CreateDistanceBasedAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("zone-based")]
    public async Task<IActionResult> CreateZoneBased(
        [FromBody] CreateZoneBasedRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await farePolicyAppService.CreateZoneBasedAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await farePolicyAppService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}