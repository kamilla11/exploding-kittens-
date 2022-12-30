using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using Kittens.Services;
using KittensLibrary;
using System.Text.Json;
using Protocol.Converter;
using Protocol;
using Protocol.Packets;
using Android.Service.Voice;

namespace Kittens.ViewModel;


public partial class GameViewModel: BaseViewModel
{
    private Client _client;
    private Player _player;
    private int _otherCardsCount;
    public ObservableCollection<Player> Players { get; } = new();
   // public Command GetPlayersCommand { get; }
    public GameViewModel()
    {
        Title = "Game";
        Status = "";
        _client = new Client(OnPacketRecieve);
    }

    public void ConnectToGameCommand(string userName, Player player)
    {
        _player = player;
        Status = "Ожидание подключения";
        _client.Connect("127.0.0.1", 4910);
        
        _client.SendHandshakePacket(userName);
        while (_player.State != State.Wait || _player.State != State.Play) { }
    }











    private void OnPacketRecieve(byte[] packet)
    {
        var parsed = Packet.Parse(packet);

        if (parsed != null)
        {
            ProcessIncomingPacket(parsed);
        }
    }

    private void ProcessIncomingPacket(Packet packet)
    {
        var type = PacketTypeManager.GetTypeFromPacket(packet);

        switch (type)
        {
            case PacketType.Handshake:
                ProcessHandshake(packet);
                break;
            case PacketType.FailConnect:
                ProcessFailConnect(packet);
                break;
            case PacketType.StartGame:
                ProcessStartGame(packet);
                break;
            case PacketType.SeeTheFuture:
                ProcessSeeTheFuture(packet);
                break;
            case PacketType.PlayerState:
                ProcessPlayerState(packet);
                break;
            case PacketType.Unknown:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ProcessPlayerState(Packet packet)
    {
        var playerState = PacketConverter.Deserialize<PacketPlayerState>(packet);
        _player.Cards = playerState.Cards;
        _player.State = playerState.PlayerState;
        _otherCardsCount = playerState.OtherPlayerCardsCount;
    }

    private void ProcessSeeTheFuture(Packet packet)
    {
        var threeCards = PacketConverter.Deserialize<PacketSeeTheFuture>(packet);
    }
    private void ProcessHandshake(Packet packet)
    {
        Status = "Успешное подключение, ожидаем еще одного игрока";    
    }
    private void ProcessFailConnect(Packet packet)
    {
        var failConnect = PacketConverter.Deserialize<PacketFailConnect>(packet);
        Status = $"{failConnect.Exception}";
    }
    private void ProcessStartGame(Packet packet)
    {
        var startGame = PacketConverter.Deserialize<PacketStartGame>(packet);
        _player = startGame.Player;
        _otherCardsCount = startGame.OtherPlayerCardsCount;
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