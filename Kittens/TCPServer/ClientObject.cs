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
    private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

    public ClientObject(Socket tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        
        Task.Run((Action) ProcessIncomingPackets);
        Task.Run((Action) SendPackets);
    }

    private void ProcessIncomingPackets()
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
                case PacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessHandshake(Packet packet)
        {
            Console.WriteLine("Recieved handshake packet.");

            var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
            handshake.MagicHandshakeNumber -= 15;
            handshake.Id = Id;
            UserName = handshake.UserName;
            
            
            Console.WriteLine($"Answering handshake packet for user: Id = {Id} UserName = {UserName}");

            QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake, handshake).ToPacket());
        }
        
        private void Process(Packet packet)
        {
            Console.WriteLine("Recieved handshake packet.");

            var handshake = PacketConverter.Deserialize<PacketHandshake>(packet);
            handshake.MagicHandshakeNumber -= 15;
            
            
            Console.WriteLine("Answering..");

            QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake, handshake).ToPacket());
        }

        public void QueuePacketSend(byte[] packet)
        {
            if (packet.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(packet);
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
}