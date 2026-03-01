namespace DungeonExplorer.Api.Domain;

public class Dungeon : IDungeon
{
    public Dungeon()
    { }

    public Dungeon(IDungeon copy)
    {
        Id = copy.Id;
        Width = copy.Width;
        Height = copy.Height;
        StartPosition = copy.StartPosition;
        Goal = copy.Goal;
        Walls = [.. copy.Walls];
    }

    public int? Id { get; set; }

    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public Position StartPosition { get; set; } = new Position();
    
    public Position Goal { get; set; } = new Position();
    
    public List<Wall> Walls { get; set; } = [];
}
