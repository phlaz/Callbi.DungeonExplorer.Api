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
        queue.Enqueue([map.Start]);

        while(queue.Count > 0)
        {
            var path = queue.Dequeue();
            var current = path.Last();

            if(current.X == map.Goal.X && current.Y == map.Goal.Y)
                return new PathResult { Path = path };

            foreach(var neighbour in GetNeighbors(current, map))
            {
                if(!visited.Contains((neighbour.X, neighbour.Y)))
                {
                    visited.Add((neighbour.X, neighbour.Y));
                    var newPath = new List<Position>(path) { neighbour };
                    queue.Enqueue(newPath);
                }
            }
        }

        return new PathResult { Error = "No valid path found" };
    }

    private static IEnumerable<Position> GetNeighbors(Position position, IDungeon map)
    {
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        foreach(var (dx, dy) in directions)
        {
            var nx = position.X + dx;
            var ny = position.Y + dy;
            if(nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height &&
                !map.Obstacles.Any(o => o.X == nx && o.Y == ny))
            {
                yield return new Position { X = nx, Y = ny };
            }
        }
    }
}