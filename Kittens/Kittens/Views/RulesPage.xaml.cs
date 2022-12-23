namespace Kittens.Views;

public partial class RulesPage : ContentPage
{
    private int countPage = 1;

    public RulesPage()
    {
        InitializeComponent();


        ImageView1.Source = ImageSource.FromFile($"rule{countPage}.png");
    }

    private void OnLeftClicked(object sender, EventArgs e)
    {
        if(countPage != 1)
        {
            countPage--;
            ImageView1.Source = ImageSource.FromFile($"rule{countPage}.png");
        }
           

        //Shell.Current.GoToAsync(nameof(MenuPage), true);
    }

    private void OnRightClicked(object sender, EventArgs e)
    {
        if(countPage!=6)
        {
            countPage++;
            ImageView1.Source = ImageSource.FromFile($"rule{countPage}.png");
        }
    }
}