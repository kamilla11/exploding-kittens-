
﻿using System.Net.Sockets;
using System.Text.Json;
//using Android.Views;
using Kittens.ViewModel;
using KittensLibrary;
//using static Java.Util.Jar.Attributes;
using Kittens.Services;
using CommunityToolkit.Maui.Views;

namespace Kittens.Views;

public partial class GamePage : ContentPage
{
    private Action<string,Player> Action;

    public Player Player { get; set; }

    GameViewModel _gameViewModel;


    public GamePage(GameViewModel gameViewModel)
	{
		InitializeComponent();
        table.IsVisible = false;
        BindingContext = new GameViewModel();
       // BindingContext = gameViewModel;
        _gameViewModel = gameViewModel;
       
    }

    private void OnOkClickedAsync(object sender, EventArgs e)
    {   
        Player = new Player(NicknameEntr.Text, EmailEntr.Text);
        _gameViewModel.Status = "j;blfyb";

        _gameViewModel.ConnectToGameCommand(Player.Nickname, Player);

        /*Action = _gameViewModel.ConnectToGameCommand;
        
        Task.Run(() => Action.Invoke(Player.Nickname, Player));*/
        login_form.IsVisible = false;
        table.IsVisible = true;  
    }

    public void SeeTheFuture(Card card1, Card card2, Card card3)
    {
        this.ShowPopup(new FuturePopUpPage());
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