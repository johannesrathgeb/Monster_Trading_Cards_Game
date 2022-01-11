using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game
{
    public class CardDeck
    {
        public List<ICard> cards = new List<ICard>();
        DB database = DB.getInstance();
        public CardDeck(List<ICard> cards)
        {
            this.cards = cards;
        }

        public CardDeck()
        {

        }
        public bool addCard(ICard card, int userID)
        {
            if (cards.Contains(card))
            {
                return false;
            }
            else
            {
                database.Connect();
                database.addCardToDeck(card.id, userID);
                
                cards.Add(card);
                return true;
            }
        }

        public bool addCardInBattle(ICard card)
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
                Console.WriteLine(card.name + " Damage: " + card.damage);
            }
            Console.Write("Press any Key to continue...");
            Console.ReadKey();
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
        public void clearDeck(int userID)
        {
            database.Connect();
            foreach (ICard card in cards)
            {
                database.removeCardFromDeck(card.id, userID);
            }
            database.Disconnect();
            cards.Clear();
        }

        public void clearBattleDeck()
        {
            cards.Clear();
        }
    }
}