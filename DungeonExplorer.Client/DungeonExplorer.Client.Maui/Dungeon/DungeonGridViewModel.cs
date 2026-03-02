using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

namespace DungeonExplorer.Client.Maui.Dungeon;

public partial class DungeonGridViewModel : ObservableObject
{
    [ObservableProperty] private int width = 10;
    [ObservableProperty] private int height = 10;
    [ObservableProperty] private ObservableCollection<Cell> cells = new();

    //public ObservableCollection<Cell> Cells => cells;

    public DungeonGridViewModel()
    {
        GenerateGrid();
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