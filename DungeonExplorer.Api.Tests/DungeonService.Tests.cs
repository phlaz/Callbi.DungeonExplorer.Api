namespace DungeonExplorer.Api.Tests;

public class MapServiceTests
{
    static private DungeonService CreateService()
    {
        var options = new DbContextOptionsBuilder<DungeonDBContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new DungeonDBContext(options);
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
            Width = 20,
            Height = 2,
            Start = new Position { X = 0, Y = 0 },
            Goal = new Position { X = 1, Y = 1 }
        };

        await Assert.ThrowsAsync<System.ArgumentException>(() => service.AddNewDungeonAsync(map));
    }

    
    [Fact]
    public void ObstacleInsideDungeon_ReturnsTrue()
    {
        var dungeon = new Dungeon { Width = 10, Height = 10 };
        var obstacle = new Obstacle { X = 5, Y = 5 };
        DungeonService service = CreateService();

        Assert.True(service.IsWithinDungeon(dungeon, new(obstacle.X, obstacle.Y)));
    }

    [Fact]
    public void ObstacleOutsideDungeon_ReturnsFalse()
    {
        var dungeon = new Dungeon { Width = 10, Height = 10 };
        var obstacle = new Obstacle { X = 10, Y = 5 }; // X is equal to Width, outside
        DungeonService service = CreateService();

        Assert.False(service.IsWithinDungeon(dungeon, new(obstacle.X, obstacle.Y)));
    }

    [Fact]
    public void ObstacleNegativeCoordinates_ReturnsFalse()
    {
        var dungeon = new Dungeon { Width = 10, Height = 10 };
        var obstacle = new Obstacle { X = -1, Y = 0 };
        DungeonService service = CreateService();

        Assert.False(service.IsWithinDungeon(dungeon, new (obstacle.X, obstacle.Y)));
    }

    [Fact]
    public void ObstacleOnBoundary_ReturnsTrue()
    {
        var dungeon = new Dungeon { Width = 10, Height = 10 };
        var obstacle = new Obstacle { X = 0, Y = 9 }; // valid boundary
        DungeonService service = CreateService();

        Assert.True(service.IsWithinDungeon(dungeon, new(obstacle.X, obstacle.Y)));
    }
}
