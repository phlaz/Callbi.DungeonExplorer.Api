namespace DungeonExplorer.Api.Tests;

public class SyncWithTests
{
    [Fact]
    public void SyncWith_Should_Handle_Multiple_New_Items_With_Id_Zero()
    {
        // Arrange: existing obstacles in the dungeon
        var existing = new List<Obstacle>
        {
            new() { Id = 1, X = 5, Y = 5 },
            new() { Id = 2, X = 6, Y = 6 }
        };

            // Incoming obstacles: two new ones (Id = 0) and one update
            var incoming = new List<Obstacle>
        {
            new() { Id = 1, X = 10, Y = 10 }, // update existing
            new() { Id = 0, X = 1, Y = 1 },   // new
            new() { Id = 0, X = 2, Y = 2 }    // new
        };

        // Act
        existing.SyncWith<Obstacle, int>(
            incoming,
            (existingObstacle, incomingObstacle) =>
            {
                existingObstacle.X = incomingObstacle.X;
                existingObstacle.Y = incomingObstacle.Y;
            });

        // Assert
        existing.Should().HaveCount(3);

        // Updated existing
        existing.Should().Contain(w => w.Id == 1 && w.X == 10 && w.Y == 10);

        // New items added
        existing.Should().Contain(w => w.Id == 0 && w.X == 1 && w.Y == 1);
        existing.Should().Contain(w => w.Id == 0 && w.X == 2 && w.Y == 2);
    }
}