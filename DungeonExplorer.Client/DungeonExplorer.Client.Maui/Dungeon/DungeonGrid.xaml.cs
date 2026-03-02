namespace DungeonExplorer.Client.Maui.Dungeon;

public partial class DungeonGrid : ContentPage
{
    public DungeonGrid(DungeonGridViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}