namespace DungeonExplorer.Api.Domain;

public class Wall
{
    public int Id { get; set; }

    public int X { get; set; }
    
    public int Y { get; set; }

    // Dungeon Foreign key
    public int DungeonId { get; set; }
}