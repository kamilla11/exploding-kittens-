using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KittensLibrary
{
    public static class Cards
    {
        public readonly static Dictionary<string, Card> cards = new Dictionary<string, Card>()
        {
            { "Exploding Kitten", new Card("Exploding Kitten", CardType.ExplodingKitten, "/cards/exploding_kitten.png")},
            { "Defuse", new Card("Defuse", CardType.Defuse, "/cards/defuse.png")},
            { "Attack", new Card("Attack", CardType.Attack, "/cards/attack.png")},
            { "Skip", new Card("Skip", CardType.Skip, "/cards/skip.png")},
            { "Shuffle", new Card("Shuffle", CardType.Shuffle, "/cards/shuffle.png")},
            { "Steal", new Card("Steal", CardType.Steal, "/cards/steal.png")},
            { "See the future", new Card("See the future", CardType.SeeTheFuture, "/cards/see_future.png")},
        };
    }
}
