namespace DungeonExplorer.Api.Storage;

public class DungeonContextFactory : IDesignTimeDbContextFactory<DungeonDBContext>
{
    public DungeonDBContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.json + environment variables
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString(Strings.DefaultConnection);

        var optionsBuilder = new DbContextOptionsBuilder<DungeonDBContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new DungeonDBContext(optionsBuilder.Options);
    }

}