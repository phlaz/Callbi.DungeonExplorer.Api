using Swashbuckle.AspNetCore.Annotations;

using System.Text.Json;

namespace DungeonExplorer.Api.Controllers;

public record ApiError(string Error, object? Details);

[ApiController]
[Route("api/dungeons")]
public class DungeonController(IDungeonService service, ILoggerFactory loggerFactory) : ControllerBase
{
    private readonly ILogger logger = loggerFactory.CreateLogger<DungeonController>();

    /// <summary>
    /// Creates and stores a new Dungeon 
    /// </summary>
    /// <param name="dungeon">Dungeon</param>
    /// <returns>Http 201 if successful, else 400</returns>
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


    /// <summary>
    /// Retrieve a dungeon by ID
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>Http 200 if found, else 404</returns>
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


    /// <summary>
    /// Get the path through a Dungeon. Positions are streamed asynchronously.
    /// </summary>
    [HttpGet("{id}/path")]
    [Produces("application/x-ndjson")]
    [SwaggerResponse(StatusCodes.Status200OK, "Path computed successfully", typeof(IAsyncEnumerable<Position>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Path computation failed", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task StreamPath(int id)
    {
        // A05: Injection – validate input
        if(id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Invalid dungeon ID", null)) + "\n");
            return;
        }

        try
        {
            var result = await service.GetRouteThroughDungeonAsync(id);

            if(result == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Dungeon not found",null)) + "\n");
                return;
            }

            if(result.Error != null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Path computation failed", result.Error)) + "\n");
                return;
            }

            await foreach(var p in result.Path.ToAsyncEnumerable())
            {
                var json = JsonSerializer.Serialize(p, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // A02: Security Misconfiguration – consistent JSON naming
                });

                await Response.WriteAsync(json + "\n");
                await Response.Body.FlushAsync(); // stream immediately
            }
        }
        catch(Exception ex)
        {
            // A09: Logging & Monitoring – log unexpected errors
            logger.LogError(ex, "Unexpected error computing path for dungeon {DungeonId}", id);

            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Unexpected server error", null)) + "\n");
        }
    }
}