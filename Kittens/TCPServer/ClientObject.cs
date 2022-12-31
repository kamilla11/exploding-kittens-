using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.Json;
using GameLogic;
using KittensLibrary;
using Protocol;
using Protocol.Converter;
using Protocol.Packets;

namespace TCPServer;

public class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal string UserName { get; set; }
    protected internal string Email { get; set; }
    protected internal State State { get; set; }


    protected internal Game Game { get; set; }

    Socket client;
    private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();
    ServerObject server; // объект сервера

    public ClientObject(Socket tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;

        Task.Run((Action)ProcessIncomingPackets);
        Task.Run((Action)SendPackets);
    }



    private void ProcessHandshake(Packet packet)
    {
        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
        UserName = handshake.UserName;
        Email = handshake.Email;
        QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake, handshake).ToPacket());
    }

    public void ProcessFailConnect(Packet packet)
    {
        QueuePacketSend(packet.ToPacket());
    }

    public void ProcessActionCard(Packet packet)
    {
        var actionCard = PacketConverter.Deserialize<PacketActionCard>(packet);
        Game.Stack.Push(Cards.cards[actionCard.ActionCard]);
        Game.playersCards[Id].RemoveAt(Game.playersCards[Id].FindIndex(0, Game.playersCards[Id].Count, card => card.Name == actionCard.ActionCard));


        switch (actionCard.ActionCard)
        {
            case "Shuffle":
                ProcessShuffle(actionCard.ActionCard);
                break;
            case "See the future":
                ProcessSeeTheFuture();
                break;
            case "Steal":
                ProcessSteal(actionCard.ActionCard);
                break;
            case "Skip":
                ProcessSkip(actionCard.ActionCard);
                break;
        }
    }

    public void ProcessSkip(string cardName)
    {
        SendPacketPlayerState(cardName, true);
    }

    public void ProcessSteal(string cardName)
    {
        var otherId = Game.playersCards.Keys.Where(key => key != Id).First();
        var otherCards = Game.playersCards[otherId];
        var random = new Random();
        var index = random.Next(0, otherCards.Count - 1);
        var stealCard = otherCards[index];
        otherCards.RemoveAt(index);
        Game.playersCards[Id].Add(stealCard);
        SendPacketPlayerState(cardName, false);
    }

    public void ProcessShuffle(string cardName)
    {
        Game.Shuffle();
        SendPacketPlayerState(cardName, false);
    }

    public void ProcessSeeTheFuture()
    {
        var threeCards = Game.Deck.Take(3).Select(card => card.Type).ToList();
        SendPacketPlayerState("See the future",false);
        QueuePacketSend(PacketConverter.Serialize(PacketType.SeeTheFuture, new PacketSeeTheFuture() { ThreeFirstCards = threeCards }).ToPacket());
    }

    public void ProcessStartGame(Packet packet)
    {
        QueuePacketSend(packet.ToPacket());
    }

    private void SendPacketPlayerState(string cardName, bool turnChange)
    {
        for (var i = 0; i < server._clients.Count; i++)
        {
            State state = server._clients[i].State;
            if (turnChange)
            {
                if (state == State.Wait) state = State.Play;
                else state = State.Wait;
            }
            server._clients[i].State = state;
            var otherCards = i == 0 ? Game.playersCards[server._clients[1].Id].Count : Game.playersCards[server._clients[0].Id].Count;
            server._clients[i].QueuePacketSend(PacketConverter.Serialize(PacketType.PlayerState, new PacketPlayerState() { Cards = Game.playersCards[server._clients[i].Id].Select(card => card.Type).ToList(), OtherPlayerCardsCount = otherCards, LastResetCard = Cards.cards[cardName].Type , PlayerState = state }).ToPacket());
        }

    }
    public void ProcessTakeCard()
    {
        var card = Game.Deck[0];
        Game.Deck.RemoveAt(0);
        if (card.Type != CardType.ExplodingKitten)
        {
            Game.playersCards[Id].Add(card);
            SendPacketPlayerState(Game.Stack.Peek().Name, true);
        }
        else
        {
            QueuePacketSend(PacketConverter.Serialize(PacketType.ExplodingKitten, new PacketExplodingKitten() { State = State.Lose }).ToPacket());
            var otherClient = server._clients.Where(c => c.Id != Id).First();
            otherClient.QueuePacketSend(PacketConverter.Serialize(PacketType.ExplodingKitten, new PacketExplodingKitten() { State = State.Win }).ToPacket());
        }
    }


    public void ProcessIncomingPackets()
    {
        
        while (true) // Слушаем пакеты, пока клиент не отключится.
        {
            var buff = new byte[256]; // Максимальный размер пакета - 256 байт.
            client.Receive(buff);

            buff = buff.TakeWhile((b, i) =>
            {
                if (b != 0xFF) return true;
                return buff[i + 1] != 0;
            }).Concat(new byte[] {0xFF, 0}).ToArray();

            var parsed = Packet.Parse(buff);

            if (parsed != null)
            {
                ProcessIncomingPacket(parsed);
            }
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
            case PacketType.ActionCard:
                ProcessActionCard(packet);
                break;
            case PacketType.TakeCard:
                ProcessTakeCard();
                break;
            case PacketType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SendPackets()
    {
        while (true)
        {
            if (_packetSendingQueue.Count == 0)
            {
                Thread.Sleep(100);
                continue;
            }

            var packet = _packetSendingQueue.Dequeue();
            client.Send(packet);

            Thread.Sleep(100);
        }
    }
    public void QueuePacketSend(byte[] packet)
    {
        if (packet.Length > 256)
        {
            throw new Exception("Max packet size is 256 bytes.");
        }

        _packetSendingQueue.Enqueue(packet);
    }

    public void Close()
    {
        client.Close();
    }
}