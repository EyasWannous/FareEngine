using FareEngine.Application.Modifications;
using FareEngine.Application.Modifications.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FareEngine.API.Controllers.Modifications;

[ApiController]
[Route("api/modifications")]
public sealed class ModificationController(IModificationAppService modificationAppService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var modifications = await modificationAppService.GetAllAsync(cancellationToken);
        return Ok(modifications);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var modification = await modificationAppService.GetByIdAsync(id, cancellationToken);
        return modification is null ? NotFound() : Ok(modification);
    }

    [HttpPost("first-class")]
    public async Task<IActionResult> CreateFirstClass(
        [FromBody] CreateFirstClassRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await modificationAppService.CreateFirstClassAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPost("senior-discount")]
    public async Task<IActionResult> CreateSeniorDiscount(
        [FromBody] CreateSeniorDiscountRequestDto input,
        CancellationToken cancellationToken)
    {
        var id = await modificationAppService.CreateSeniorDiscountAsync(input, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await modificationAppService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
