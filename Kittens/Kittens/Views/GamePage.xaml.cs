
﻿using System.Net.Sockets;
using System.Text.Json;
//using Android.Views;
using Kittens.ViewModel;
using KittensLibrary;
//using static Java.Util.Jar.Attributes;
using Kittens.Services;

namespace Kittens.Views;

public partial class GamePage : ContentPage
{
    //string Host { get; } = "127.0.0.1";
    //int Port { get; } = 8888;
    //TcpClient Client { get; } = new();
    //StreamReader Reader { get; set; }
    //StreamWriter Writer { get; set; }

    private GameConnect _gameService;

    public Player Player { get; set; }

    GameViewModel _gameViewModel;


    public GamePage(GameViewModel gameViewModel)
	{
		InitializeComponent();
        table.IsVisible = false;
        BindingContext = gameViewModel;
        _gameViewModel = gameViewModel;
        _gameService = new GameConnect();
    }

    private async void OnOkClickedAsync(object sender, EventArgs e)
    {
        login_form.IsVisible = false;
        table.IsVisible = true;
        Player = new Player(NicknameEntr.Text, EmailEntr.Text);
        var player = JsonSerializer.Serialize(Player);
       // GameService.ConnectPlayer +=
        await _gameViewModel.ConnectToGameCommand(player, (player) => AddPlayer(player));

        //var name = JsonSerializer.Deserialize<Player>(player).Nickname;
        //Title += $"{name} ";

        //if (Client.Client.Connected)
        //    return;

        //login_form.IsVisible = false;

        

        //try
        //{
        //    Client.Connect(Host, Port); 
        //    Reader = new StreamReader(Client.GetStream());
        //    Writer = new StreamWriter(Client.GetStream());
        //    if (Writer is null || Reader is null) return;
        //    Task.Run(() => ReceiveMessageAsync(Reader));
        //    var player = JsonSerializer.Serialize(Player);
        //    // ��������� ���� ���������
        //    await SendMessageAsync(Writer, player);
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

    }

    public async void AddPlayer(string name)
    {
        Title += $"{name} ";
    }


    //async Task SendMessageAsync(StreamWriter writer, string message)
    //{
    //    await writer.WriteLineAsync(message);
    //    await writer.FlushAsync();
    //}

    //async Task ReceiveMessageAsync(StreamReader reader)
    //{
    //    while (true)
    //    {
    //        try
    //        {
    //            // ��������� ����� � ���� ������
    //            string? message = await reader.ReadLineAsync();
    //            // ���� ������ �����, ������ �� ������� �� �������
    //            if (string.IsNullOrEmpty(message)) continue;

    //            if (message.StartsWith("user_connect"))
    //            {
    //                var player = JsonSerializer.Deserialize<Player>(message.Replace("user_connect", ""));
    //                AddPlayer(player.Nickname);

    //                //playersList.Invoke(() =>
    //                //{
    //                //    var label = new Label();
    //                //    label.Text = user.UserName;
    //                //    label.ForeColor = Color.FromArgb(user.Color);
    //                //    playersList.Controls.Add(label);
    //                //});
    //            }
    //            else if (message.StartsWith("user_action"))
    //            {
    //                var sendAction = JsonSerializer.Deserialize<SendAction>(message.Replace("user_action", ""));
    //                //метод обновления данных на экране
    //                //Draw(sendPoint.Point, Color.FromArgb(sendPoint.Color));
    //            }
    //            //playersList.Items.Add(message);
    //        }
    //        catch
    //        {
    //            break;
    //        }
    //    }
    //}

    //public void Draw(Point point, Color color)
    //{
    //    var p = new Pen(color, 5);

    //    Rectangle r = new Rectangle();
    //    r.Width = 4;
    //    r.Height = 4;
    //    r.Location = new Point(point.X - r.Width / 2, point.Y - r.Height / 2);
    //    g.DrawEllipse(p, r);
    //}

    //private async void SendPoint(object sender, EventArgs e)
    //{
    //    MouseEventArgs mouseArgs = e as MouseEventArgs;

    //    var json = JsonSerializer.Serialize(new SendPoint { UserName = UserName, Point = mouseArgs.Location });
    //    await SendMessageAsync(Writer, json);
    //}

}