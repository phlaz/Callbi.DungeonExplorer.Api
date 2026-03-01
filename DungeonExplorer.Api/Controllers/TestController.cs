namespace DungeonExplorer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("throw")]
    public IActionResult ThrowException()
    {
        throw new InvalidOperationException("Simulated failure");
    }
}