namespace DungeonExplorer.Client.Maui.Dungeon;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsWall { get; set; }
    public bool IsStart { get; set; }
    public bool IsGoal { get; set; }
}