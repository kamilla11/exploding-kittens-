using System.Net.Sockets;
using System.Text.Json;
using Protocol;
using Protocol.Converter;

namespace TCPServer;

public class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal string UserName { get; set; }
    Socket client;
    ServerObject server; // объект сервера

    public ClientObject(Socket tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
    }

    public async Task ProcessIncomingPackets()
    {
        while (true) // Слушаем пакеты, пока клиент не отключится.
        {
            var buff = new byte[256]; // Максимальный размер пакета - 256 байт.
            await client.ReceiveAsync(buff,SocketFlags.None);

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
        byte[] newPacket = new byte[1];

        Console.WriteLine("Recieved handshake packet.");

        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
        handshake.MagicHandshakeNumber -= 15;
        handshake.Id = Id;
        UserName = handshake.UserName;


        Console.WriteLine("Answering..");
        newPacket = PacketConverter.Serialize(PacketType.Handshake, handshake).ToPacket();
        
        
        if (newPacket.Length > 256)
        {
            throw new Exception("Max packet size is 256 bytes.");
        }

        client.Send(newPacket, SocketFlags.None);
    }
    public void ProcessFailConnect(Packet packet)
    {
        
        var newPacket = packet.ToPacket();
        if (newPacket.Length > 256)
        {
            throw new Exception("Max packet size is 256 bytes.");
        }

        client.Send(newPacket, SocketFlags.None);
    }
}