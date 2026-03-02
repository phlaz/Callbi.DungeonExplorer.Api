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

    public async Task<IDungeon?> UpdateWallsAsync(int dungeonId, List<Wall> walls)
    {
        //reconnect the entity to the change tracker
        var dungeon = await GetDungeonAsync(dungeonId);
        if(dungeon == null) return null;

        var incoming = walls.Select(w => new Wall
        {
            Id = w.Id,
            X = w.X,
            Y = w.Y,
            DungeonId = dungeonId
        }).ToList();

        //Synchronize the walls collection.
        dungeon.Walls.SyncWith<Wall, int>(
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