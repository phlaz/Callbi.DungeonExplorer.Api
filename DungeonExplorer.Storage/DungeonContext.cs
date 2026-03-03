namespace DungeonExplorer.Api.Storage;

public class DungeonContext(DbContextOptions<DungeonContext> options) : DbContext(options)
{
    public DbSet<Dungeon> Dungeons { get; set; }

    public DbSet<Obstacle> Obstacles { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Owned<Position>();
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.Start);
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.Goal);
        modelBuilder.Entity<Dungeon>().HasMany(d => d.Obstacles).WithOne().HasForeignKey(w => w.DungeonId);

        base.OnModelCreating(modelBuilder);
    }
}