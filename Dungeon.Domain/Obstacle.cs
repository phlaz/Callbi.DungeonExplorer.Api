namespace DungeonExplorer.Api.Domain;

public class Obstacle : IIdentifiable<int>
{
    public int Id { get; set; }

    public int X { get; set; }
    
    public int Y { get; set; }

    // Dungeon Foreign key
    public int DungeonId { get; set; }

    public int CompareTo(IIdentifiable<int>? other) => CompareTo(other);
}