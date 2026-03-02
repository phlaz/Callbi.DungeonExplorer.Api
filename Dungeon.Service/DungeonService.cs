namespace DungeonExplorer.Api.Service;

public class DungeonService(IDungeonRepository repository, IPathfindingService pathfinding)
    : IDungeonService
{
    public async Task<bool> AddNewDungeonAsync(IDungeon map)
    {
        // Validation
        if(map.Width < 5 || map.Width > 50 || map.Height < 5 || map.Height > 50)
            throw new ArgumentException("Invalid grid size");

        return await repository.AddNewDungeonAsync(map);
    }

    public async Task<IDungeon?> GetDungeonAsync(int id) => await repository.GetDungeonAsync(id);

    public async Task<PathResult> GetRouteThroughDungeonAsync(int id)
    {
        var dungeon = await repository.GetDungeonAsync(id);
        if(dungeon == null) return new PathResult { Error = "Dungeon not found" };
        return pathfinding.ComputePath(dungeon);
    }

    public async Task<IDungeon?> UpdateObstaclesAsync(int dungeonId, List<Obstacle> walls)
    {
        //reconnect the entity to the change tracker
        var dungeon = await GetDungeonAsync(dungeonId);
        if(dungeon == null) return null;

        var incoming = walls.Select(w => new Obstacle
        {
            Id = w.Id,
            X = w.X,
            Y = w.Y,
            DungeonId = dungeonId
        }).ToList();

        //Synchronize the walls collection.
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
}