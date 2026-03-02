namespace DungeonExplorer.Client.Maui.Dungeon;

public partial class DungeonPage : ContentPage
{
	public DungeonPage(DungeonViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}