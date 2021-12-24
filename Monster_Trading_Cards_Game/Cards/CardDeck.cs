using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class CardDeck
    {
        List<ICard> cards = new List<ICard>();

        public CardDeck(List<ICard> cards)
        {
            this.cards = cards;
        }

        public CardDeck()
        {

        }
        public bool addCard(ICard card)
        {
            if (cards.Contains(card))
            {
                return false;
            }
            else
            {
                cards.Add(card);
                return true;
            }
        }

        public void printDeck()
        {
            foreach(ICard card in cards)
            {
                Console.WriteLine(card.name);
            }
        }

        public ICard randomCard()
        {
            Random random = new Random();
            int number = random.Next(0, cards.Count()-1);

            return cards[number];
        }
        public void removeCard(ICard card)
        {
            cards.Remove(card);
        }

        public int cardCount()
        {
            return cards.Count();
        }
        public void clearDeck()
        {
            cards.Clear();
        }
    }
}