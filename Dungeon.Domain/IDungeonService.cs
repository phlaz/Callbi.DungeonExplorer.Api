namespace DungeonExplorer.Api.Domain;

public interface IDungeonService
{
    Task<bool> AddNewDungeonAsync(IDungeon model);

    Task<IDungeon?> GetDungeonAsync(int id);

    Task<PathResult> GetRouteThroughDungeonAsync(int id);

    Task<IDungeon?> UpdateWallsAsync(int id, List<Wall> walls);
}


