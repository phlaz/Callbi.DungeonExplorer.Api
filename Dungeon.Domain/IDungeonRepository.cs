namespace DungeonExplorer.Api.Domain;

public interface IReadDungeonRepository<TDungeon>
    where TDungeon : IDungeon
{
    Task<TDungeon?> GetDungeonAsync(int id);
}

public interface IWriteDungeonRepository<in TDungeon>
    where TDungeon : IDungeon
{
    Task<bool> AddNewDungeonAsync(TDungeon model);
}

public interface IDungeonRepository
    : IReadDungeonRepository<IDungeon>, IWriteDungeonRepository<IDungeon>
{ }


