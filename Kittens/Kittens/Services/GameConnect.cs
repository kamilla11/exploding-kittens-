using System.Net.Sockets;
using System.Text.Json;
using KittensLibrary;
using Kittens.Views;

namespace Kittens.Services;

public class GameConnect
{

    string Host { get; } = "127.0.0.1";
    int Port { get; } = 8888;
    TcpClient Client { get; } = new();
    StreamReader Reader { get; set; }
    StreamWriter Writer { get; set; }

   // private List<Player> players;
    public event Action<string> ConnectPlayer; 
    
    public GameConnect()
    {
        
    }

    //public async Task<IEnumerable<Player>>  GetPlayers()
    //{
    //    return null;
    //}

    public async Task SendMessageAsync(StreamWriter writer, string message)
    {
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }

    public async void ConnectAsync(string message)
    {
        if (Client.Client.Connected)
            return;

        try
        {
            Client.Connect(Host, Port); 
            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream());
            if (Writer is null || Reader is null) return;
            
           
            Task.Run(() => ReceiveMessageAsync(Reader));
            await SendMessageAsync(Writer, message);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    async Task ReceiveMessageAsync(StreamReader reader)
    {
        while (true)
        {
            try
            {
                string? message = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(message)) continue;

                if (message.StartsWith("user_connect"))
                {
                    var player = JsonSerializer.Deserialize<Player>(message.Replace("user_connect", ""));

                   // players.Add(player);

                    ConnectPlayer(player.Nickname);
                   

                    //AddPlayer(player.Nickname);

                    //playersList.Invoke(() =>
                    //{
                    //    var label = new Label();
                    //    label.Text = user.UserName;
                    //    label.ForeColor = Color.FromArgb(user.Color);
                    //    playersList.Controls.Add(label);
                    //});
                }
                else if (message.StartsWith("user_action"))
                {
                    //var sendPoint = JsonSerializer.Deserialize<SendPoint>(message.Replace("user_action", ""));
                    //Draw(sendPoint.Point, Color.FromArgb(sendPoint.Color));
                }
                //playersList.Items.Add(message);
            }
            catch
            {
                break;
            }
        }
    }

}