using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    public class CardStack
    {
        public List<ICard> cards = new List<ICard>();

        public CardStack(List<ICard> cardList)
        {
            this.cards = cardList;
        }

        public ICard searchCard(ICard inputCard)
        {
            foreach(ICard card in cards)
            {
                if(card == inputCard)
                {
                    return card;
                }
            }
            return null;
        }

        public bool isCardInStack(int id)
        {
            foreach (ICard card in cards)
            {
                if (card.id == id)
                {
                    return true;
                }
            }
            return false;
        }
        public void printList()
        {
            foreach (ICard card in cards)
            {                
                Console.WriteLine("[ ] " + card.name + " Damage: " + card.damage);
            }
        }

        public void addCardToStack(ICard card)
        {
            cards.Add(card);
        }
    }
}
