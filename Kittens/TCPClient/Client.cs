using System.Net;
using System.Net.Sockets;
using Protocol;
using Protocol.Converter;

namespace TCPClient;

public class Client
{
    private int _handshakeMagic;
    public string UserName { get; set; }

    private Socket _socket;
    private IPEndPoint _serverEndPoint;

    public Client(string userName)
    {
        UserName = userName;
    }

    public void Connect(string ip, int port)
    {
        Connect(new IPEndPoint(IPAddress.Parse(ip),port));
    }
    
    public void Connect(IPEndPoint server)
    {
        _serverEndPoint = server;
        
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(_serverEndPoint);

        Task.Run(() => RecievePackets());
    }
    

    private async Task RecievePackets()
    {
        while (true)
        {
            var buff = new byte[256];
            await _socket.ReceiveAsync(buff,SocketFlags.None);

            buff = buff.TakeWhile((b, i) =>
            {
                if (b != 0xFF) return true;
                return buff[i + 1] != 0;
            }).Concat(new byte[] {0xFF, 0}).ToArray();

            OnPacketRecieve(buff);
        }
    }

    public void SendPacket(Packet packet)
    {
        _socket.Send(packet.ToPacket());
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
            case PacketType.Unknown:
                break;
            case PacketType.FailConnect:
                ProcessFailConnect(packet);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ProcessHandshake(Packet packet)
    {
        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);

        if (_handshakeMagic - handshake.MagicHandshakeNumber == 15)
        {
            Console.WriteLine($"Handshake successful! Player Id = {handshake.Id}");
        }
    }
    private void ProcessFailConnect(Packet packet)
    {
        var failConnect = PacketConverter.Deserialize<PacketFailConnect>(packet);

        Console.WriteLine(failConnect.Exception);
    }
}