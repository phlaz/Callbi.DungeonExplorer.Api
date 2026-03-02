namespace DungeonExplorer.Client.Maui.Dungeon;

public class Dungeon
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Point StartPosition { get; set; } = new();
    public Point Goal { get; set; } = new();
    public List<Point> Walls { get; set; } = new();
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}