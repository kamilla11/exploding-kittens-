using Protocol;
using Protocol.Converter;
using TCPClient;

public class Program
{
    private static int _handshakeMagic;

    private static void Main()
    {

        var client = new Client("User1");
        client.OnPacketRecieve += OnPacketRecieve;
        client.Connect("127.0.0.1", 4910);

        var rand = new Random();
        _handshakeMagic = rand.Next();

        Thread.Sleep(1000);
            
        Console.WriteLine("Sending handshake packet..");

        client.QueuePacketSend(
            PacketConverter.Serialize(
                    PacketType.Handshake,
                    new PacketHandshake
                    {
                        MagicHandshakeNumber = _handshakeMagic,
                        Id = "",
                        UserName = client.UserName
                    })
                .ToPacket());

        while(true) {}
    }

    private static void OnPacketRecieve(byte[] packet)
    {
        var parsed = Packet.Parse(packet);

        if (parsed != null)
        {
            ProcessIncomingPacket(parsed);
        }
    }

    private static void ProcessIncomingPacket(Packet packet)
    {
        var type = PacketTypeManager.GetTypeFromPacket(packet);

        switch (type)
        {
            case PacketType.Handshake:
                ProcessHandshake(packet);
                break;
            case PacketType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static void ProcessHandshake(Packet packet)
    {
        var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);

        if (_handshakeMagic - handshake.MagicHandshakeNumber == 15)
        {
            Console.WriteLine($"Handshake successful! Player Id = {handshake.Id}");
        }
    }
}