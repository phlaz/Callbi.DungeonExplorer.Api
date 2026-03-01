namespace DungeonExplorer.Api.Storage;

public class DungeonRepository(DungeonContext context) : IDungeonRepository
{
    public async Task<bool> AddNewDungeonAsync(IDungeon dungeon)
    {
        context.Dungeons.Add((Dungeon)dungeon);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IDungeon?> GetDungeonAsync(int id)
    {
        return await context.Dungeons.Include(m => m.Walls).FirstOrDefaultAsync(m => m.Id == id);
    }
}
