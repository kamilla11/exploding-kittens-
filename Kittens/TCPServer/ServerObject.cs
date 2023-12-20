using System.Net;
using System.Net.Sockets;
using GameLogic;
using KittensLibrary;
using Protocol;
using Protocol.Converter;
using Protocol.Packets;

namespace TCPServer;

public class ServerObject
{
    private readonly Socket _socket;
    internal readonly List<ClientObject> _clients;

    private bool _listening;
    private bool _stopListening;

    public ServerObject()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _clients = new List<ClientObject>();
    }

    public void Start()
    {
        if (_listening)
        {
            throw new Exception("Сервер уже запущен");
        }

        _socket.Bind(new IPEndPoint(IPAddress.Any, 4910));
        _socket.Listen();

        _listening = true;
    }

    public void Stop()
    {
        if (!_listening)
        {
            throw new Exception("Сервер уже выключен");
        }

        _stopListening = true;
        _socket.Shutdown(SocketShutdown.Both);
        _listening = false;
    }
    
    public void AcceptClients()
    {
        while (true)
        {
            if (_stopListening)
            {
                return;
            }

            Socket client;

            try
            {
                client = _socket.Accept();
            } 
            catch(Exception ex) {Console.WriteLine(ex.Message); return; }

            Console.WriteLine($"[!] Accepted client from {(IPEndPoint) client.RemoteEndPoint}");
            var c = new ClientObject(client,this);
            if (_clients.Count < 2)
            {
                _clients.Add(c);
                Task.Run(() => c.ProcessIncomingPackets());
            }
            else
            {
                c.ProcessFailConnect(PacketConverter.Serialize(PacketType.FailConnect, new PacketFailConnect()
                {
                    Exception = "К игре могут присоединиться только 2 игрока"
                }));
                c.Close();
            }
            if (_clients.Count == 2)
            {
                var game = new Game();
                _clients[0].Game = game;
                _clients[1].Game = game;
                _clients[0].State = State.Play;
                var playerCards1 = game.GetCardsForPlayer();
                game.playersCards.Add(_clients[0].Id, playerCards1);
                _clients[0].ProcessStartGame(PacketConverter.Serialize(PacketType.StartGame, new PacketStartGame() { Player = new Player(_clients[0].Id, _clients[0].UserName, _clients[0].Email, playerCards1.Select(card => card.Type).ToList(), State.Play), OtherPlayerCardsCount = 8 }));
                var playerCards2 = game.GetCardsForPlayer();
                _clients[1].State = State.Wait;
                game.playersCards.Add(_clients[1].Id, playerCards2);
                _clients[1].ProcessStartGame(PacketConverter.Serialize(PacketType.StartGame, new PacketStartGame() { Player = new Player(_clients[1].Id, _clients[1].UserName, _clients[0].Email, playerCards2.Select(card => card.Type).ToList(), State.Wait), OtherPlayerCardsCount = 8 }));

            }
            
        }
    }
}