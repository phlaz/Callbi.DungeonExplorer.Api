namespace DungeonExplorer.Api.Tests;

public class PathfindingTests
{
    [Fact]
    public void PathExists_WhenGoalReachable()
    {
        var map = new Dungeon
        {
            Width = 5,
            Height = 5,
            StartPosition = new Position { X = 0, Y = 0 },
            Goal = new Position { X = 4, Y = 4 },
            Walls = []
        };

        var service = new PathfindingService();
        var result = service.ComputePath(map);

        Assert.NotNull(result.Path);
        Assert.True(result.Path.Count > 0);
        Assert.Null(result.Error);
    }

    [Fact]
    public void NoPath_WhenGoalBlocked()
    {
        var map = new Dungeon
        {
            Width = 5,
            Height = 5,
            StartPosition = new Position { X = 0, Y = 0 },
            Goal = new Position { X = 1, Y = 0 },
            Walls = [ new Wall{ X = 1, Y = 0 } ]
        };

        var service = new PathfindingService();
        var result = service.ComputePath(map);

        Assert.NotNull(result.Error);
        Assert.Empty(result.Path);
    }
}