namespace DungeonExplorer.Api.Domain;

public class PathResult
{
    public List<Position> Path { get; set; } = [];

    public string? Error { get; set; }
}