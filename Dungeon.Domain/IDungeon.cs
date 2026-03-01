namespace DungeonExplorer.Api.Domain;

public interface IDungeon
{
    int? Id { get; }

    int Width { get; }

    int Height { get; }

    Position StartPosition { get; }

    Position Goal { get; }

    List<Wall> Walls { get; }

}

