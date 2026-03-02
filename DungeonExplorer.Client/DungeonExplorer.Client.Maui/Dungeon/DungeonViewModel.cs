using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DungeonExplorer.Client.Maui.Dungeon;

public partial class DungeonViewModel : ObservableObject
{
    private readonly DungeonService service;

    [ObservableProperty] private int width = 10;
    [ObservableProperty] private int height = 10;
    [ObservableProperty] private int startX = 0;
    [ObservableProperty] private int startY = 0;
    [ObservableProperty] private int goalX = 9;
    [ObservableProperty] private int goalY = 9;
    [ObservableProperty] private string resultMessage = string.Empty;

    public DungeonViewModel(DungeonService service)
    {
        this.service = service;
    }


    [RelayCommand]
    public async Task CreateDungeonAsync()
    {
        var dungeon = new Dungeon
        {
            Width = Width,
            Height = Height,
            Start = new Point { X = StartX, Y = StartY },
            Goal = new Point { X = GoalX, Y = GoalY },
            Walls = new List<Point>() // leave empty for now
        };

        var result = await service.CreateDungeon(dungeon);
        ResultMessage = result != null ? $"Dungeon created: {result.Width}x{result.Height}" : "Error creating dungeon";
    }
}