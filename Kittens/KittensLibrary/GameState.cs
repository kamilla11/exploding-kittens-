using System;
namespace KittensLibrary
{
  

    public class GameState
    {
        //колода
        public List<Card> Deck { get; set; }
        //верхняя карта сброса
        public Card TopResetCard { get; set; }
        //карты у каждого игрока
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
       
    }
}

