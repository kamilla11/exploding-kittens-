using System.Net.Sockets;
using Protocol;
using TCPServer;

internal class Program
{
    private async static Task Main()
    {
        var server = new ServerObject();
        server.Start();
        server.AcceptClients();
    }
}