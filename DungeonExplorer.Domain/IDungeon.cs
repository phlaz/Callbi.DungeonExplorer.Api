namespace DungeonExplorer.Api.Domain;

public interface IDungeon
{
    int? Id { get; }

    int Width { get; }

    int Height { get; }

    Position Start { get; }

    Position Goal { get; }

    List<Obstacle> Obstacles { get; }
}

