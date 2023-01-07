
using Protocol;
using Protocol.Converter;
using System.Net;
using System.Net.Sockets;

namespace Kittens;

public class Client
{

    public Socket _socket;
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
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        _socket.Connect(_serverEndPoint);

        try
        {
            Task.Run(() => RecievePackets());
            Task.Run(() => SendPackets());
        } 
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
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


    private async Task RecievePackets()
    {
        while (true)
        {
            var buff = new byte[260];
            await _socket.ReceiveAsync(buff, SocketFlags.None);

           
            buff = buff.TakeWhile((b, i) =>
            {
                if (b != 0xFF) return true;
                return buff[i + 1] != 0;
            }).Concat(new byte[] {0xFF, 0}).ToArray();

            OnPacketRecieve?.Invoke(buff);
        }
    }
    private async Task  SendPackets()
    {
        while (true)
        {
            if (_packetSendingQueue.Count == 0)
            {
                continue;
            }

            var packet = _packetSendingQueue.Dequeue();
            await _socket.SendAsync(packet,SocketFlags.None);

        }
    }
    public void SendHandshakePacket(string userName, string email)
    {
        QueuePacketSend(PacketConverter.Serialize(PacketType.Handshake, new PacketHandshake() { UserName = userName , Email = email}).ToPacket());
    }
    
    
}