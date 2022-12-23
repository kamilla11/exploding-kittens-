using Kittens.ViewModel;

namespace Kittens.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel gameViewModel)
	{
		InitializeComponent();
		BindingContext = gameViewModel;
	}
}