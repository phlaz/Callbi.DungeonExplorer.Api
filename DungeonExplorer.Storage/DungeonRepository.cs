namespace DungeonExplorer.Api.Storage;
public class DungeonRepository(DungeonDBContext context) : IDungeonRepository
{
    public async Task<bool> AddNewDungeonAsync(Dungeon dungeon)
    {
        context.Dungeons.Add((Dungeon)dungeon);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Dungeon?> GetDungeonAsync(int id)
    {
        return await context.Dungeons.Include(m => m.Obstacles).FirstOrDefaultAsync(m => m.Id == id);
    }

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
