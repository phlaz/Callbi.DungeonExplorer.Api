namespace DungeonExplorer.Api.Domain;

public interface IDungeonService
{
    Task<bool> AddNewDungeonAsync(Dungeon model);

    Task<Dungeon?> GetDungeonAsync(int id);

    Task<PathResult> GetRouteThroughDungeonAsync(int id);

    Task<Dungeon?> UpdateObstaclesAsync(int id, List<Obstacle> obstacles);
}


