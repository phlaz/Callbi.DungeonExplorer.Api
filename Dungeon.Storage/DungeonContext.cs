using DungeonExplorer.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DungeonExplorer.Api.Storage;

public class DungeonContext(DbContextOptions<DungeonContext> options) : DbContext(options)
{
    public DbSet<DungeonMap> Dungeons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Treat Position as a value object
        modelBuilder.Owned<Position>();

        base.OnModelCreating(modelBuilder);
    }

}