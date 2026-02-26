namespace Dungeon.Storage;

using Dungeon.Domain;

using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

public class DungeonRepository(DungeonContext context) : IDungeonRepository
{
    public async Task<IDungeon?> AddNewDungeonAsync(IDungeon dungeon)
    {
        context.Dungeons.Add(new DungeonMap(dungeon));
        await context.SaveChangesAsync();
        return dungeon;
    }


    public async Task<IDungeon?> GetDungeonAsync(int id)
    {
        return await context.Dungeons.FirstOrDefaultAsync(m => m.Id == id);
    }

}
