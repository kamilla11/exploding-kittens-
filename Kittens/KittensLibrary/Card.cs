namespace KittensLibrary;

public class Card : CardType
{
    public string Img { get; set; }
}

public class CardType
{
    public CardName CardName { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
}

public enum CardName
{
    Attack,
    Defuse,
    Nope,
    Shuffle,
    Skip,
    Kitten,
    ExplodingKitten,
    SeeTheFuture,
    Favor
}