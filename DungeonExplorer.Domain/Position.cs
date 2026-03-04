namespace DungeonExplorer.Api.Domain;

public class Position 
{
    public Position()
    { }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    [Range(0, 50, ErrorMessage = "X must be between 0 and 50"), JsonPropertyName("x")]
    public int X { get; set; }

    [Range(0, 50, ErrorMessage = "Y must be between 0 and 50"), JsonPropertyName("y")]
    public int Y { get; set; }
}
