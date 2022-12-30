using System;
using CommunityToolkit.Mvvm.ComponentModel;
using KittensLibrary;
using System.Collections.ObjectModel;

namespace Kittens.ViewModel
{
	public partial class FutureViewModel: BaseViewModel
	{
        [ObservableProperty]
        ObservableCollection<Card> cards;


    }
}

