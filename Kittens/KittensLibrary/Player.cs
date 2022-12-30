namespace KittensLibrary;

public enum State
{
    WaitGame,
    StartGame,
    Win,
    Lose,
    Play,
    Wait
}

public class Player
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public List<Card> Cards { get; set; }
    public State State { get; set; }


    public Player(string nickname, string email)
    {
        Nickname = nickname;
        Email = email;
    }
}

