using KittensLibrary;
using Microsoft.VisualBasic;

namespace GameLogic
{
    public class Game
    {

        public List<Card> Deck = new List<Card>() {
             Cards.cards["Defuse"], Cards.cards["Defuse"], Cards.cards["Defuse"], Cards.cards["Defuse"],
             Cards.cards["Attack"], Cards.cards["Attack"], Cards.cards["Attack"], Cards.cards["Attack"],
             Cards.cards["Skip"], Cards.cards["Skip"], Cards.cards["Skip"],  Cards.cards["Skip"],
             Cards.cards["Shuffle"], Cards.cards["Shuffle"], Cards.cards["Shuffle"], Cards.cards["Shuffle"],
             Cards.cards["Steal"], Cards.cards["Steal"], Cards.cards["Steal"], Cards.cards["Steal"],
             Cards.cards["See the future"], Cards.cards["See the future"], Cards.cards["See the future"], Cards.cards["See the future"], Cards.cards["See the future"],
         };

        /*public List<Card> Deck = new List<Card>() {

            //Cards.cards["Exploding Kitten"],
            Cards.cards["Defuse"], Cards.cards["Defuse"],
            Cards.cards["Attack"], Cards.cards["Attack"],
            Cards.cards["Skip"], Cards.cards["Skip"], Cards.cards["Skip"],
            Cards.cards["Shuffle"], Cards.cards["Shuffle"], Cards.cards["Shuffle"],
            Cards.cards["Steal"], Cards.cards["Steal"], Cards.cards["Steal"],
            Cards.cards["See the future"], Cards.cards["See the future"],
        };*/

        public Stack<Card> Stack = new Stack<Card>();

        public Dictionary<string, List<Card>> playersCards = new Dictionary<string, List<Card>>();

        public Game()
        {
            Shuffle();
            Shuffle();
            
        }

        public List<Card> GetCardsForPlayer()
        {
            var playerCards = new List<Card>() { Cards.cards["Defuse"] };

            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                var newCard = random.Next(0, Deck.Count - 1);
                playerCards.Add(Deck[newCard]);
                Deck.RemoveAt(newCard);
            }
            Shuffle();
            return playerCards;
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
        }




    }
}