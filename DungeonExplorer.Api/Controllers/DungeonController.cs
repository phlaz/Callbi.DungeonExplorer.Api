namespace DungeonExplorer.Api.Controllers;

public record ApiError(string Error, object? Details);

[ApiController]
[Route("api/dungeons"), Authorize()]
public class DungeonController(IDungeonService service, ILoggerFactory loggerFactory, IWebHostEnvironment environment) : ControllerBase
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
        try
        {
            ArgumentNullException.ThrowIfNull(dungeon, nameof(dungeon));
        
            await service.AddNewDungeonAsync(dungeon);

            var result = CreatedAtAction(nameof(Create), new { id = dungeon.Id }, dungeon);
            logger.LogInformation("New Dungeon created, Id: {id}", dungeon.Id);
            return result;
        }
        catch(ArgumentException x)
        {
            return BadRequest(new { error = x.Message });
        }
        catch(Exception x)
        {
            logger.LogError(x, x.Message);
            var response = environment.IsDevelopment()
                ? new { error = "Unexpected server error", details = x.Message }
                : new { error = "Unexpected server error", details = "Please try again later." };

            return StatusCode(500, response);
        }
    }

   
    [HttpPatch("{id}/obstacles")]
    [SwaggerResponse(StatusCodes.Status200OK, "Obstacles updated successfully", typeof(Dungeon))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid obstacle data", typeof(ApiError))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error", typeof(ApiError))]
    public async Task<IActionResult> UpdateObstacles(int id, [FromBody] List<Obstacle> obstacles)
    {
        try
        {
            if(obstacles is null)
            {
                logger.LogWarning("Obstacles is null. Nothing to update");
                //no data to update shouldn't cause an error
                return Ok();
            }
        
            if(id <= 0)
            {
                logger.LogWarning("Dungeon ID is not valid ({id})", id);
                return BadRequest(new ApiError("Dungeon not found", null));
            }

            logger.LogInformation("Updating obstacles for Dungeon {id}", id);
            var updatedDungeon = await service.UpdateObstaclesAsync(id, obstacles);
            if(updatedDungeon == null)
            {
                logger.LogWarning("Dungeon ID {id} not found", id);
                return NotFound(new ApiError("Dungeon not found", null));
            }

            return Ok(updatedDungeon);
        }
        catch(Exception x)
        {
            logger.LogError(x, x.Message);
            var response = environment.IsDevelopment()
                ? new { error = "Unexpected server error", details = x.Message }
                : new { error = "Unexpected server error", details = "Please try again later." };

            return StatusCode(500, response);
        }
    }


    /// <summary>
    /// Get the path through a Dungeon. Positions are streamed asynchronously.
    /// </summary>
    [HttpGet("{id}/path")]
    [Produces("application/x-ndjson")]
    [SwaggerResponse(StatusCodes.Status200OK, "Path computed successfully", typeof(IAsyncEnumerable<Position>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Path computation failed")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Dungeon not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected server error")]
    public async Task StreamPath(int id)
    {
        
        if(id <= 0)
        {
            logger.LogWarning("Dungeon ID is not valid ({id})", id);
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Invalid dungeon ID", null) + "\n"));
            return;
        }

        logger.LogInformation("Calculating path through Dungeon {id}", id);

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = await service.GetRouteThroughDungeonAsync(id);

            if(result == null)
            {
                logger.LogWarning("Dungeon ID {id} not found", id);
                Response.StatusCode = StatusCodes.Status404NotFound;
                await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Dungeon not found", null) + "\n"));
                return;
            }

            if(result.Error != null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;

                logger.LogWarning("Path computation failed: {x}", result.Error);
                var apiError = new ApiError("Path computation failed", result.Error);

                var json = JsonSerializer.Serialize(apiError, options);

                await Response.WriteAsync(json + "\n");
                return;
            }

            await foreach(var p in result.Path.ToAsyncEnumerable())
            {
                var json = JsonSerializer.Serialize(p, options);

                await Response.WriteAsync(json + "\n");
                await Response.Body.FlushAsync();
            }
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Unexpected error computing path for dungeon {DungeonId}", id);

            Response.StatusCode = StatusCodes.Status500InternalServerError;
            await Response.WriteAsync(JsonSerializer.Serialize(new ApiError("Unexpected server error", null) + "\n"));
        }
    }
}