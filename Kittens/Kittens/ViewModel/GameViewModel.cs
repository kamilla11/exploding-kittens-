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
using CommunityToolkit.Mvvm.ComponentModel;

namespace Kittens.ViewModel;


public partial class GameViewModel: BaseViewModel
{

    public event Action<Card, Card, Card> SeeTheFutureUI;
    public event Action ExplodingKittenUI;
    public event Action DefuseUI;
    public event Action AttackUI;
    public event Action SkipUI;
    public event Action NopeUI;
    public event Action ShuffleUI;
    public event Action StealUI;

    private Client _client;
    private Player _player;

    private Card _draggedCard;

    private Card _backCard = new Card("back", CardType.Back, "card_back.png");

    [ObservableProperty]
    ObservableCollection<Card> playerCards;

    [ObservableProperty]
    ObservableCollection<Card> otherPlayerCards;

    [ObservableProperty]
    Card lastResetCard;


    public GameViewModel()
    {
        Title = "Game";
        Status = "";
        _client = new Client(OnPacketRecieve);
        PlayerCards = new ObservableCollection<Card>();
    }

    private void UpdateOtherPlayerCards(int count)
    {
        for (int i = 0; i < count; i++)
            OtherPlayerCards.Add(_backCard);
    }

    [RelayCommand]
    public void DragStarted(Card card)
    {
        _draggedCard = card;
    }

    [RelayCommand]
    public void DragBackStarted(Card card)
    {
        _draggedCard = card;
    }

    [RelayCommand]
    public void CardDropedToReset()
    {
        if(_draggedCard.Type != CardType.Back)
        {
            //чето делаем, отправляем пакет об отправленной карте, обновляем сброс
        }
    }

    [RelayCommand]
    public void BackCardDroped()
    {
        if (_draggedCard.Type == CardType.Back)
        {
            //добавляем игроку новую карту из колоды
        }
    }

    
    //[RelayCommand]
    //void UpdateCards()
    //{

    //}

    //private void AddCardList()
    //{

    //}

    public void ConnectToGameCommand(string userName, Player player)
    {
        _player = player;
        Status = "Ожидание подключения";
        _client.Connect("127.0.0.1", 4910);
        
        _client.SendHandshakePacket(userName);
        while (_player.State != State.StartGame) { }
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
            case PacketType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessHandshake(Packet packet)
    {
        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
        _player.Id = handshake.Id;
        Status = "Успешное подключение, ожидаем еще одного игрока";
        
    }

    private void ProcessFailConnect(Packet packet)
    {
        var failConnect = PacketConverter.Deserialize<PacketFailConnect>(packet);

        Status = $"{failConnect.Exception}";
    }

    private void ProcessStartGame(Packet packet)
    {
        var failConnect = PacketConverter.Deserialize<PacketStartGame>(packet);
        Status = $"{failConnect.Status}";
        _player.State = State.StartGame;
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