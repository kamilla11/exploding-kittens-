using Protocol;
using Protocol.Converter;
using System.Net;
using System.Net.Sockets;

namespace Kittens;

public class Client
{

    private Socket _socket;
    private IPEndPoint _serverEndPoint;
    private Action<byte[]> OnPacketRecieve;
    private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();
    public string Id { get; set; }

    public Client(Action<byte[]> onPacketRecieve)
    {
        OnPacketRecieve = onPacketRecieve;
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

        Task.Run((Action)RecievePackets);
        Task.Run((Action)SendPackets);
    }

    public void QueuePacketSend(byte[] packet)
    {
        if (packet.Length > 256)
        {
            throw new Exception("Max packet size is 256 bytes.");
        }

        _packetSendingQueue.Enqueue(packet);
    }


    private void RecievePackets()
    {
        while (true)
        {
            var buff = new byte[256];
            _socket.Receive(buff,SocketFlags.None);

            buff = buff.TakeWhile((b, i) =>
            {
                if (b != 0xFF) return true;
                return buff[i + 1] != 0;
            }).Concat(new byte[] {0xFF, 0}).ToArray();

            OnPacketRecieve?.Invoke(buff);
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
            _socket.Send(packet);

            Thread.Sleep(100);
        }
    }
    public void SendHandshakePacket(string userName)
    {
        QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake, new PacketHandshake() { UserName = userName }).ToPacket());
    }
    
    
}