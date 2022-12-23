using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Kittens.Services;

namespace Kittens.ViewModel;

public class Player
{
    
}

public partial class GameViewModel: BaseViewModel
{
    private GameService _gameService;
    public ObservableCollection<Player> Players { get; } = new();
   // public Command GetPlayersCommand { get; }
    public GameViewModel(GameService gameService)
    {
        Title = "Game";
        _gameService = new GameService();
        // GetPlayersCommand = new Command(async () => await GetPlayersAsync);
    }

    [RelayCommand]
    async Task GetPlayersAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var players = await _gameService.GetPlayers();

            if (Players.Count != 0)
                Players.Clear();

            foreach (var player in players)
                Players.Add(player);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }
}