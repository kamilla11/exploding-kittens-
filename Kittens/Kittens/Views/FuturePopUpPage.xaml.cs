using CommunityToolkit.Maui.Views;
using KittensLibrary;

namespace Kittens.Views;

public partial class FuturePopUpPage : Popup
{
	private List<Card> _cards;
	public FuturePopUpPage(List<Card> cards)
	{
		InitializeComponent();
		_cards = cards;

        Image image0 = new Image
        {
            Source = cards[0].Img
        };

        Image image1 = new Image
        {
            Source = cards[1].Img
        };

        Image image2 = new Image
        {
            Source = cards[2].Img
        };

        //img0.Source = cards[0].Img;
        //img1.Source = cards[1].Img;
        //img2.Source = cards[2].Img;
    }
}