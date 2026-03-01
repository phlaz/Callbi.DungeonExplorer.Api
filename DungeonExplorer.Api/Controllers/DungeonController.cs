namespace DungeonExplorer.Api.Controllers;

[ApiController]
[Route("api/dungeons")]
public class DungeonController(IDungeonService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Dungeon dungeon)
    {
        ArgumentNullException.ThrowIfNull(dungeon, nameof(dungeon));
        if(!await service.AddNewDungeonAsync(dungeon))
            return BadRequest();
        return CreatedAtAction(nameof(Get), new { id = dungeon.Id }, dungeon);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var dungeon = await service.GetDungeonAsync(id);
        if(dungeon == null) return NotFound();
        return Ok(dungeon);
    }

    [HttpGet("{id}/path")]
    public async Task<IActionResult> GetPath(int id)
    {
        var result = await service.GetRouteThroughDungeonAsync(id);
        if(result.Error != null) return BadRequest(result);
        return Ok(result);
    }
}