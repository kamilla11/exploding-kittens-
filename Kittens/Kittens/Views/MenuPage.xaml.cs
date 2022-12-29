namespace Kittens.Views;

public partial class MenuPage : ContentPage
{

	public MenuPage()
	{
		InitializeComponent();
	}

    private async void OnStartGameClicked(object sender, EventArgs e)
    {
       await Shell.Current.GoToAsync(nameof(GamePage), true);
    }

    private async void OnRulesClicked(object sender, EventArgs e)
    {
       await Shell.Current.GoToAsync(nameof(RulesPage), true);
    }
}


