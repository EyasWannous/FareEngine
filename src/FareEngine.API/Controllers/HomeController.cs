using Microsoft.AspNetCore.Mvc;

namespace FareEngine.API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
    
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok();
    }
}