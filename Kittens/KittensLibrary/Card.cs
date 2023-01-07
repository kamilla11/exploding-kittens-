using System;
namespace KittensLibrary
{
	public enum CardType
	{
		Defuse,
        ExplodingKitten,
        Attack,
		Skip,
		Nope,
		Shuffle,
		Steal,
		SeeTheFuture,
		Back,
		None
	}
	public class Card
	{
		public string? Name { get; set; }
		public CardType Type { get; set; }
		public string? Img { get; set; }

		public Card(string name, CardType type, string img)
		{
			Name = name;
			Type = type;
			Img = img;
		}

        public Card()
        {
        }
    }
}