using Dungeon.Domain;

namespace Dungeon.Service;

public class DungeonService(IDungeonRepository repository, IPathfindingService pathfinding)
    : IDungeonService
{
    public async Task<IDungeon?> AddNewDungeonAsync(IDungeon map)
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
}