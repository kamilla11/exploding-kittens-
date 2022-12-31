namespace KittensLibrary;

public enum State
{
    WaitGame,
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
    public List<CardType> Cards { get; set; }
    public State State { get; set; }


    public Player(string nickname, string email)
    {
        Nickname = nickname;
        Email = email;
        State = State.WaitGame;
    }
    public Player(string id, string nickname, string email, List<CardType> cards, State state)
    {
        Id = id;
        Nickname = nickname;
        Email = email;
        Cards = cards;
        State = state;
    }

    public Player()
    {
    }
}

