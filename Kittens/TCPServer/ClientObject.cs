using System.Net.Sockets;
using System.Text.Json;
using GameLogic;
using Protocol;
using Protocol.Converter;

namespace TCPServer;

public class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal string UserName { get; set; }
    protected internal string Email { get; set; }

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
            case PacketType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessHandshake(Packet packet)
    {
        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
        UserName = handshake.UserName;
        Email = handshake.Email;
        QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake,handshake).ToPacket());
    }

    public void ProcessFailConnect(Packet packet)
    { 
        QueuePacketSend(packet.ToPacket());
    }
    public void ProcessStartGame(Packet packet)
    {
        QueuePacketSend(packet.ToPacket());
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