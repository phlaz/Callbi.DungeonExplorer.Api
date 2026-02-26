using DungeonExplorer.Api.Domain;

namespace DungeonExplorer.Api.Service;

public interface IPathfindingService
{
    PathResult ComputePath(IDungeon map);
}

public class PathfindingService : IPathfindingService
{
    public PathResult ComputePath(IDungeon map)
    {
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<List<Position>>();
        queue.Enqueue([map.StartPosition]);

        while(queue.Count > 0)
        {
            var path = queue.Dequeue();
            var current = path.Last();

            if(current.X == map.Goal.X && current.Y == map.Goal.Y)
                return new PathResult { Path = path };

            foreach(var neighbor in GetNeighbors(current, map))
            {
                if(!visited.Contains((neighbor.X, neighbor.Y)))
                {
                    visited.Add((neighbor.X, neighbor.Y));
                    var newPath = new List<Position>(path) { neighbor };
                    queue.Enqueue(newPath);
                }
            }
        }

        return new PathResult { Error = "No valid path found" };
    }

    private static IEnumerable<Position> GetNeighbors(Position pos, IDungeon map)
    {
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        foreach(var (dx, dy) in directions)
        {
            var nx = pos.X + dx;
            var ny = pos.Y + dy;
            if(nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height &&
                !map.Walls.Any(o => o.X == nx && o.Y == ny))
            {
                yield return new Position { X = nx, Y = ny };
            }
        }
    }
}