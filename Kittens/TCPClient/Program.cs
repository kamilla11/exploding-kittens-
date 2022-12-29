using Protocol;
using Protocol.Converter;
using TCPClient;

public class Program
{

    private static void Main()
    {

        var client = new Client("User1");
        client.Connect("127.0.0.1", 4910);
        

        /*Thread.Sleep(1000);
            
        Console.WriteLine("Sending handshake packet..");

        client.SendPacket(
            PacketConverter.Serialize(
                    PacketType.Handshake,
                    new PacketHandshake
                    {
                        MagicHandshakeNumber = _handshakeMagic,
                        Id = "",
                        UserName = client.UserName
                    }));*/

        while(true) {}
    }

    
    
}