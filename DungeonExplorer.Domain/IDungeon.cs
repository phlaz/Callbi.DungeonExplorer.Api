namespace DungeonExplorer.Api.Domain;

public interface IDungeon
{
    int Id { get; set; }

    int Width { get; set; }

    int Height { get; set; }

    Position Start { get; set; }

    Position Goal { get; set; }

    ICollection<Obstacle> Obstacles { get; set; }
}

