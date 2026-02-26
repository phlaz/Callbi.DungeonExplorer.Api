namespace Dungeon.Domain;

public interface IDungeonService
{
    Task<IDungeon?> AddNewDungeonAsync(IDungeon model);

    Task<IDungeon?> GetDungeonAsync(int id);

    Task<PathResult> GetRouteThroughDungeonAsync(int id);
}

public interface IPosition
{
    int X { get; }
    
    int Y { get; }
}

public interface IDungeon
{
    int Id { get; }

    int Width { get; }

    int Height { get; }

    IPosition StartPosition { get; }

    IPosition Goal { get; }

    IPosition[] Walls { get; }
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

public class Class1
{

}

