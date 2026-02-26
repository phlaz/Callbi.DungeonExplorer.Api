namespace DungeonExplorer.Api.Domain;

public interface IDungeonService
{
    Task<IDungeon?> AddNewDungeonAsync(IDungeon model);

    Task<IDungeon?> GetDungeonAsync(int id);

    Task<PathResult> GetRouteThroughDungeonAsync(int id);
}

public interface IDungeon
{
    int Id { get; }

    int Width { get; }

    int Height { get; }

    Position StartPosition { get; }

    Position Goal { get; }

    Position[] Walls { get; }
}

public interface IDungeonBuilder
{
    Task<int> AddNewDungeonAsync(IDungeon model);

    Task<IDungeon> GetDungeonAsync(int id);
}

public interface IDungeonRepository : IReadDungeonRepository<IDungeon>, IWriteDungeonRepository<IDungeon>
{
}

public interface IReadDungeonRepository<TDungeon>
    where TDungeon : IDungeon
{
    Task<TDungeon?> GetDungeonAsync(int id);
}

public interface IWriteDungeonRepository<in TDungeon>
    where TDungeon : IDungeon
{
    Task<IDungeon?> AddNewDungeonAsync(TDungeon model);
}


