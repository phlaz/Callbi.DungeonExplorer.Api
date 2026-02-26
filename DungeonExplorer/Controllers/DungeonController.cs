using Microsoft.AspNetCore.Mvc;
using DungeonExplorer.Api.Service;
using DungeonExplorer.Api.Domain;

namespace DungeonExplorer.Api.Controllers;

[ApiController]
[Route("api/dungeons")]
public class DungeonController(DungeonService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IDungeon dungeon)
    {
        ArgumentNullException.ThrowIfNull(dungeon, nameof(dungeon));
        var created = await service.AddNewDungeonAsync(dungeon);
        if(created == null) return BadRequest();
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
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