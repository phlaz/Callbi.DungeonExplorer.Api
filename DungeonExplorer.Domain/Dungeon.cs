

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
        Start = copy.Start;
        Goal = copy.Goal;
        Obstacles = [.. copy.Obstacles];
    }

    [Key]
    public int Id { get; set; }

    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public Position Start { get; set; } = new Position();
    
    public Position Goal { get; set; } = new Position();
    
    public ICollection<Obstacle> Obstacles { get; set; } = [];
}
