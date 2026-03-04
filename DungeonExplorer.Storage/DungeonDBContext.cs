namespace DungeonExplorer.Api.Storage;

public class DungeonDBContext(DbContextOptions<DungeonDBContext> options) : IdentityDbContext(options)
{
    public DbSet<Dungeon> Dungeons { get; set; }

    public DbSet<Obstacle> Obstacles { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Owned<Position>();
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.Start);
        modelBuilder.Entity<Dungeon>().OwnsOne(d => d.Goal);
        modelBuilder.Entity<Dungeon>().HasMany(d => d.Obstacles).WithOne().HasForeignKey(o => o.DungeonId).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}