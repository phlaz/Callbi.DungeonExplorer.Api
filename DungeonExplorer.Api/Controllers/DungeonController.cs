using Swashbuckle.AspNetCore.Annotations;

namespace DungeonExplorer.Api.Controllers;

public record ApiError(string Error, object? Details);

[ApiController]
[Route("api/dungeons")]
public class DungeonController(IDungeonService service) : ControllerBase
{
    [HttpPost]
    [SwaggerResponse(StatusCodes.Status201Created, "Dungeon created successfully", typeof(Dungeon))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid dungeon data", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task<IActionResult> Create([FromBody] Dungeon dungeon)
    {
        ArgumentNullException.ThrowIfNull(dungeon, nameof(dungeon));
        if(!await service.AddNewDungeonAsync(dungeon))
            return BadRequest(new ApiError("Validation failed", null));

        return CreatedAtAction(nameof(Get), new { id = dungeon.Id }, dungeon);
    }

    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Dungeon retrieved successfully", typeof(Dungeon))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task<IActionResult> Get(int id)
    {
        var dungeon = await service.GetDungeonAsync(id);
        if(dungeon == null) return NotFound(new ApiError("Dungeon not found", null));
        return Ok(dungeon);
    }

    [HttpGet("{id}/path")]
    [SwaggerResponse(StatusCodes.Status200OK, "Path computed successfully", typeof(PathResult))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Path computation failed", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task<IActionResult> GetPath(int id)
    {
        var result = await service.GetRouteThroughDungeonAsync(id);
        if(result.Error != null) return BadRequest(new ApiError("Path computation failed", result.Error));
        return Ok(result);
    }

    [HttpPatch("{id}/obstacles")]
    [SwaggerResponse(StatusCodes.Status200OK, "Obstacles updated successfully", typeof(Dungeon))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid obstacle data", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task<IActionResult> UpdateObstacles(int id, [FromBody] List<Obstacle> walls)
    {
        var updatedDungeon = await service.UpdateObstaclesAsync(id, walls);
        if(updatedDungeon == null)
        {
            return NotFound(new ApiError("Dungeon not found", null));
        }

        return Ok(updatedDungeon);
    }
}