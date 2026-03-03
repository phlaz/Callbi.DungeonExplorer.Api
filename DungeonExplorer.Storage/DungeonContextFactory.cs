using Microsoft.EntityFrameworkCore.Design;

namespace DungeonExplorer.Api.Storage;

public class DungeonContextFactory : IDesignTimeDbContextFactory<DungeonContext>
{
    public DungeonContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DungeonContext>();
        optionsBuilder.UseSqlite(Strings.DataSource);

        return new DungeonContext(optionsBuilder.Options);
    }
}