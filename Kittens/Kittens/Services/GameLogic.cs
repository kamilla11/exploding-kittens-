using System;
using KittensLibrary;
namespace Kittens.Services
{
    public class GameLogic
    {
        Player Player1 { get; set; }
        Player Player2 { get; set; }

        Player CurrentPlayer { get; set; }
        Player OtherPlayer { get; set; }

        Card TopResetCard { get; set; }

        public event Action<Card, Card, Card> SeeTheFutureUI;
        public event Action ExplodingKittenUI;
        public event Action<string> DefuseUI;
        public event Action<string> AttackUI;
        public event Action<string> SkipUI;
        public event Action<string> NopeUI;
        public event Action ShuffleUI;
        public event Action<string> StealUI;

        readonly static Dictionary<string, Card> cards = new Dictionary<string, Card>()
        {
            { "Exploding Kitten", new Card("Exploding Kitten", CardType.ExplodingKitten, "/cards/exploding_kitten.png")},
            { "Defuse", new Card("Defuse", CardType.Defuse, "/cards/defuse.png")},
            { "Attack", new Card("Attack", CardType.Attack, "/cards/attack.png")},
            { "Skip", new Card("Skip", CardType.Skip, "/cards/skip.png")},
            { "Nope", new Card("Nope", CardType.Nope, "/cards/Nope.png")},
            { "Shuffle", new Card("Shuffle", CardType.Shuffle, "/cards/shuffle.png")},
            { "Steal", new Card("Steal", CardType.Steal, "/cards/steal.png")},
            { "See the future", new Card("See the future", CardType.SeeTheFuture, "/cards/see_future.png")},
        };

        List<Card> Deck = new List<Card>() {
            cards["Defuse"], cards["Defuse"], cards["Defuse"], cards["Defuse"],
            cards["Nope"], cards["Nope"], cards["Nope"], cards["Nope"], cards["Nope"],
            cards["Attack"], cards["Attack"], cards["Attack"], cards["Attack"],
            cards["Skip"], cards["Skip"], cards["Skip"],  cards["Skip"],
            cards["Shuffle"], cards["Shuffle"], cards["Shuffle"], cards["Shuffle"],
            cards["Steal"], cards["Steal"], cards["Steal"], cards["Steal"],
            cards["See the future"], cards["See the future"], cards["See the future"], cards["See the future"], cards["See the future"],
        };

        public GameLogic()
        {

        }

        public void StartGame()
        {

            Player1.Cards.Add(cards["Defuse"]);
            Player2.Cards.Add(cards["Defuse"]);

            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                var val1 = random.Next(0, cards.Count);
                Player1.Cards.Add(Deck[val1]);
                Deck.RemoveAt(val1);

                var val2 = random.Next(0, cards.Count);
                Player2.Cards.Add(Deck[val2]);
                Deck.RemoveAt(val2);
            }

            Deck.Add(cards["Exploding kitten"]);
            Shuffle();
        }

        public void Shuffle()
        {
            Random random = new Random();
            for (int i = Deck.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = Deck[j];
                Deck[j] = Deck[i];
                Deck[i] = temp;
            }
            ShuffleUI();
        }

        public void SeeTheFuture()
        {
            var i = Deck.Count;
            SeeTheFutureUI(Deck[i - 1], Deck[i - 2], Deck[i - 3]);
        }

        public GameState TakeACard()
        {
            var card = Deck.Last();
            Deck.RemoveAt(Deck.Count - 1);
            if (card.Type is CardType.ExplodingKitten)
            {
                if (!CurrentPlayer.Cards.Contains(cards["Defuse"]))
                {
                    ExplodingKittenUI();
                }
                else
                {
                    Deck.Add(cards["Exploding kitten"]);
                    Shuffle();
                }
            }
            else CurrentPlayer.Cards.Add(card);


            return new GameState() { Deck = Deck, Player1 = Player1, Player2 = Player2, TopResetCard = TopResetCard };
        }

        public GameState PlayCard(Card card)
        {
            switch (card.Type)
            {
                case (CardType.SeeTheFuture):
                        SeeTheFuture();
                        break;
                case (CardType.Shuffle):
                    Shuffle();
                    break;
                case (CardType.Skip):
                    CurrentPlayer.State = State.Wait;
                    OtherPlayer.State = State.Play;
                    break;
            }
            TopResetCard = card;
            return new GameState() { Deck = Deck, Player1 = Player1, Player2 = Player2, TopResetCard = TopResetCard };
        }
    }
}

