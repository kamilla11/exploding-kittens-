using System.Net;
using System.Net.Sockets;
using Protocol;
using Protocol.Converter;

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
    
    public async Task AcceptClients()
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
                client = await _socket.AcceptAsync();
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
            }
            
            
        }
    }

    
    /*protected internal async Task ListenAsync()
    {
        try
        {
            using Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndP
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                var tcpClient = await tcpListener.AcceptAsync();
                
                ClientObject clientObject = new ClientObject(tcpClient, this);
                
                clients.Add(clientObject.Id, clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            oint(IPAddress.Any, 8888);
            tcpListener.Bind(ipPoint);   // связываем с локальной точкой ipPoint
            tcpListener.Listen();Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    // трансляция сообщения подключенным клиентам
    protected internal async Task BroadcastMessageAsync(string message, string id)
    {
        foreach (var client in clients)
        {   
            await client.Writer.WriteLineAsync(message); //передача данных
            await client.Writer.FlushAsync();
        }
    }

    // отключение всех клиентов
    protected internal void Disconnect()
    {
        foreach (var (id, client) in clients)
        {
            client.Close(); //отключение клиента
        }
        tcpListener.Stop(); //остановка сервера
    }*/
}