namespace DungeonExplorer.Api.Storage;

public class DungeonContext(DbContextOptions<DungeonContext> options) : DbContext(options)
{
    public DbSet<Dungeon> Dungeons { get; set; }

    public DbSet<Wall> Walls { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Owned<Position>();
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.StartPosition);
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.Goal);
        modelBuilder.Entity<Dungeon>().HasMany(d => d.Walls).WithOne().HasForeignKey(w => w.DungeonId);

        base.OnModelCreating(modelBuilder);
    }
}