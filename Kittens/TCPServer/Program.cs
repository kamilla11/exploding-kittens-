using System.Net.Sockets;
using Protocol;
using TCPServer;

internal class Program
{
    private static void Main()
    {
        var server = new ServerObject();
        server.Start();
        server.AcceptClients();
    }
}