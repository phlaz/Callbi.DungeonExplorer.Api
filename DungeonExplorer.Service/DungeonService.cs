[assembly: InternalsVisibleTo("DungeonExplorer.Api.Tests")]

namespace DungeonExplorer.Api.Service;

public class DungeonService(IDungeonRepository repository, IPathfindingService pathfinding)
    : IDungeonService
{
    public async Task<bool> AddNewDungeonAsync(IDungeon dungeon)
    {
        // Validation
        if(dungeon.Width < 5 || dungeon.Width > 50 || dungeon.Height < 5 || dungeon.Height > 50)
        {
            throw new ArgumentException("Invalid grid size");
        }

        return await repository.AddNewDungeonAsync(dungeon);
    }

    public async Task<IDungeon?> GetDungeonAsync(int id) => await repository.GetDungeonAsync(id);

    public async Task<PathResult> GetRouteThroughDungeonAsync(int id)
    {
        var dungeon = await repository.GetDungeonAsync(id);
        if(dungeon == null) return new PathResult { Error = "Dungeon not found" };
        return pathfinding.ComputePath(dungeon);
    }

    public async Task<IDungeon?> UpdateObstaclesAsync(int dungeonId, List<Obstacle> obstacles)
    {
        //reconnect the entity to the change tracker
        var dungeon = await GetDungeonAsync(dungeonId);
        if(dungeon == null) return null;

        var incoming = obstacles
            .Where(obstacle => IsObstacleWithinDungeon(dungeon, obstacle))
            .Select(obstacle => new Obstacle
            {
                Id = obstacle.Id,
                X = obstacle.X,
                Y = obstacle.Y,
                DungeonId = dungeonId
            }).ToList();

        //Synchronize the obstacles collection.
        dungeon.Obstacles.SyncWith<Obstacle, int>(
            incoming,
            (existing, incomingItem) =>
            {
                existing.X = incomingItem.X;
                existing.Y = incomingItem.Y;
            });

        await repository.SaveChangesAsync();
        return dungeon;
    }

    internal bool IsObstacleWithinDungeon(IDungeon dungeon, Obstacle o) => 
        o.X >= 0 && o.X < dungeon.Width && o.Y >= 0 && o.Y < dungeon.Height;
}