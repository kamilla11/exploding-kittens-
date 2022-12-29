using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Kittens.Services;
using KittensLibrary;
using System.Text.Json;

namespace Kittens.ViewModel;


public partial class GameViewModel: BaseViewModel
{
    private GameConnect _gameConnect;
    public ObservableCollection<Player> Players { get; } = new();
   // public Command GetPlayersCommand { get; }
    public GameViewModel(GameConnect gameConnect)
    {

        Title = "Game";
        _gameConnect = new GameConnect();
        // GetPlayersCommand = new Command(async () => await GetPlayersAsync);
    }

    public async Task ConnectToGameCommand(string player, Action<string> method)
    {
        //var name = JsonSerializer.Deserialize<Player>(player).Nickname;
        //Title += $"{name} ";
        _gameConnect.ConnectPlayer += method;
        _gameConnect.ConnectAsync(player);
    }

    //[RelayCommand]
    //async Task GetPlayersAsync()
    //{
    //    if (IsBusy)
    //        return;

       

    //    try
    //    {
    //        IsBusy = true;
    //        var players = await _gameService.GetPlayers();

    //        if (Players.Count != 0)
    //            Players.Clear();
    //        var playersString = new StringBuilder();

    //        foreach (var player in players)
    //        {
    //            playersString.Append(player.Nickname + " ");
    //            Players.Add(player);
    //        }
               

    //        //await Shell.Current.DisplayAlert("Список участников:",playersString.ToString(), "Ok");

    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
    //        await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
    //    }
    //    finally
    //    {
    //        IsBusy = false;
    //    }

    //}
}