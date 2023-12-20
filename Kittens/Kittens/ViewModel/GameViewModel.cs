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
using CommunityToolkit.Maui.Core.Extensions;

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
    public event Action DisableFalseUI;
    public event Action DisableTrueUI;

    private Client _client;
    private Player _player;

    private Card _draggedCard;

    private Card _backCard = new Card("back", CardType.Back, "card_back.png");

    [ObservableProperty]
    List<Card> backCard = new List<Card>() { new Card("back", CardType.Back, "card_back.png") };

    private int _otherCardsCount;

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
        OtherPlayerCards = new ObservableCollection<Card>();
    }

    private void Update()
    {
        if (_player.State == State.Play)
        {
            Title =  $"{_player.Nickname} ходит";
            DisableTrueUI();
        }
        else
        {
            Title = $"{_player.Nickname} ожидает";
            DisableFalseUI();
        }

        /* PlayerCards = _player.Cards.Select(card => Cards.typeCards[card]).ToObservableCollection();*/
        PlayerCards.Clear();
        OtherPlayerCards.Clear();

        foreach (var playerCard in _player.Cards.Select(card => Cards.typeCards[card]))
        {
            PlayerCards.Add(playerCard);
        }

        for (int i = 0; i < _otherCardsCount; i++)
        {
            OtherPlayerCards.Add(_backCard);
        }
           
    }

   
    [RelayCommand]
    async Task DragStartedAsync(Card card)
    {
        _draggedCard = card;
    }

    
    [RelayCommand]
    async Task DragBackStartedAsync(Card card)
    {
        _draggedCard = card;
    }

    //в сброс
    [RelayCommand]
    async Task CardDropedToResetAsync()
    {
        if(_draggedCard.Type != CardType.Back)
        {
            _client.QueuePacketSend(PacketConverter.Serialize(PacketType.ActionCard, new PacketActionCard() { ActionCard = _draggedCard.Name }).ToPacket());
        }
    }

    //в свои карты
    [RelayCommand]
    async Task BackCardDropedAsync()
    {
        if (_draggedCard.Type == CardType.Back)
        {
           _client.QueuePacketSend(PacketConverter.Serialize(PacketType.TakeCard, new PacketTakeCard() { Test = 0 }).ToPacket());
        }
    }

    
    //[RelayCommand]
    //void UpdateCards()
    //{

    //}

    //private void AddCardList()
    //{

    //}

    public void ConnectToGameCommand(string userName, string email)
    {
        _player = new Player(userName,email);
        Status = "Ожидание подключения";
        _client.Connect("127.0.0.1", 4910);
        
        _client.SendHandshakePacket(userName, email);
        while (_player.State != State.Play && _player.State != State.Wait) { }
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
            case PacketType.ExplodingKitten:
                ProcessExplodingKitten(packet);
                break;
            case PacketType.Unknown:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessExplodingKitten(Packet packet)
    {
        if(_player.Cards.Contains(CardType.Defuse))
        {
            _client.QueuePacketSend(PacketConverter.Serialize(PacketType.ActionCard, new PacketActionCard() { ActionCard = "Defuse" }).ToPacket());
        }
    }
    private void ProcessPlayerState(Packet packet)
    {
        var playerState = PacketConverter.Deserialize<PacketPlayerState>(packet);
        _player.Cards = playerState.Cards;
        _player.State = playerState.PlayerState;
        _otherCardsCount = playerState.OtherPlayerCardsCount;
        LastResetCard = (playerState.LastResetCard is CardType.None)?null: Cards.typeCards[playerState.LastResetCard];
        Update();
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
        _player.Cards = startGame.Player.Cards;
        _player.State = startGame.Player.State;
        _otherCardsCount = startGame.OtherPlayerCardsCount;
        Update();
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