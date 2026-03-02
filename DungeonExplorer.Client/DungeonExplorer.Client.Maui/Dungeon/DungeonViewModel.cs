using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

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

    [ObservableProperty] private ObservableCollection<Cell> cells = new();


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
            StartPosition = new Point { X = StartX, Y = StartY },
            Goal = new Point { X = GoalX, Y = GoalY },
            Walls = new List<Point>() // leave empty for now
        };

        var result = await service.CreateDungeon(dungeon);
        var message = "Error creating dungeon";
        if(result != null)
        {
            GenerateGrid();
            message = $"Dungeon created: {result.Width}x{result.Height}";
        }
        ResultMessage = message;
    }

    [RelayCommand]
    public void GenerateGrid()
    {
        Cells.Clear();
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                Cells.Add(new Cell { X = x, Y = y });
            }
        }
    }

    [RelayCommand]
    public void ToggleWall(Cell cell)
    {
        cell.IsWall = !cell.IsWall;
        OnPropertyChanged(nameof(Cells));
    }

}