namespace Kittens.Views;

public partial class MenuPage : ContentPage
{

	public MenuPage()
	{
		InitializeComponent();
	}

    private void OnStartGameClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(GamePage), true);
    }
    private void OnRulesClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(RulesPage), true);
    }
}


