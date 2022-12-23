using System;
using System.Collections.Generic;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace Kittens.Views;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    private void OnStartClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(MenuPage), true);
    }
}