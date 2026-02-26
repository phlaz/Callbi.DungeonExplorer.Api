namespace DungeonExplorer.Api.Domain;

public class Position 
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class DungeonMap : IDungeon
{
    public DungeonMap()
    { }

    public DungeonMap(IDungeon copy)
    {
        Id = copy.Id;
        Width = copy.Width;
        Height = copy.Height;
        StartPosition = copy.StartPosition;
        Goal = copy.Goal;
        Array.Copy(copy.Walls, Walls, copy.Walls.Length);
    }

    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Position StartPosition { get; set; } = new Position();
    public Position Goal { get; set; } = new Position();
    public Position[] Walls { get; set; } = [];
}

public class PathResult
{
    public List<Position> Path { get; set; } = [];
    public string? Error { get; set; }
}