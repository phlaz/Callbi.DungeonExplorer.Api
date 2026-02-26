using Dungeon.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dungeon.Storage;

public class DungeonContext(DbContextOptions<DungeonContext> options) : DbContext(options)
{
    public DbSet<DungeonMap> Dungeons { get; set; }
}