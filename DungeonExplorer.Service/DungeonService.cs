[assembly: InternalsVisibleTo("DungeonExplorer.Api.Tests")]

namespace DungeonExplorer.Api.Service;

public class DungeonService(IDungeonRepository repository, IPathfindingService pathfinding)
    : IDungeonService
{
    public async Task<bool> AddNewDungeonAsync(Dungeon dungeon)
    {
        // Validation
        if(dungeon.Width < 5 || dungeon.Width > 50 || dungeon.Height < 5 || dungeon.Height > 50)
        {
            throw new ArgumentException("Invalid grid size");
        }

        if(!IsWithinDungeon(dungeon, dungeon.Start))
        {
            throw new ArgumentException("Start is outside the dungeon");
        }

        if(!IsWithinDungeon(dungeon, dungeon.Goal))
        {
            throw new ArgumentException("Goal is outside the dungeon");
        }

        dungeon.Obstacles = [.. dungeon.Obstacles.Where(o => IsWithinDungeon(dungeon, new(o.X, o.Y)))];
        foreach(var obstacle in dungeon.Obstacles)
        {
            if(obstacle.DungeonId == 0)
            {
                obstacle.Dungeon = dungeon;
            }
        }

        return await repository.AddNewDungeonAsync(dungeon);
    }

    public async Task<Dungeon?> GetDungeonAsync(int id) => await repository.GetDungeonAsync(id);

    public async Task<PathResult> GetRouteThroughDungeonAsync(int id)
    {
        var dungeon = await repository.GetDungeonAsync(id);
        if(dungeon == null) return new PathResult { Error = "Dungeon not found" };
        return pathfinding.ComputePath(dungeon);
    }

    public async Task<Dungeon?> UpdateObstaclesAsync(int dungeonId, List<Obstacle> obstacles)
    {
        //reconnect the entity to the change tracker
        var dungeon = await GetDungeonAsync(dungeonId);
        if(dungeon == null) return null;

        var incoming = obstacles
            .Where(obstacle => IsWithinDungeon(dungeon, new(obstacle.X, obstacle.Y)))
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

    internal bool IsWithinDungeon(IDungeon dungeon, Position position) => 
        position.X >= 0 && position.X < dungeon.Width && position.Y >= 0 && position.Y < dungeon.Height;
}