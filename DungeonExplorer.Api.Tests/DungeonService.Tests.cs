namespace DungeonExplorer.Api.Tests;

public class MapServiceTests
{
    static private DungeonService CreateService()
    {
        var options = new DbContextOptionsBuilder<DungeonContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new DungeonContext(options);
        var repo = new DungeonRepository(context);
        var pathfinding = new PathfindingService();
        return new DungeonService(repo, pathfinding);
    }

    [Fact]
    public async Task CreateDungeon_ValidDungeon_Succeeds()
    {
        var service = CreateService();
        var map = new Dungeon
        {
            Width = 10,
            Height = 10,
            Start = new Position { X = 0, Y = 0 },
            Goal = new Position { X = 9, Y = 9 }
        };

        var created = await service.AddNewDungeonAsync(map);

        Assert.True(created);
        Assert.Equal(10, map.Width);
    }

    [Fact]
    public async Task CreateDungeon_InvalidSize_Throws()
    {
        var service = CreateService();
        var map = new Dungeon
        {
            Width = 2,
            Height = 2,
            Start = new Position { X = 0, Y = 0 },
            Goal = new Position { X = 1, Y = 1 }
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() => service.AddNewDungeonAsync(map));
    }
}