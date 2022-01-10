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

        public ICard searchCard(string input)
        {
            foreach(ICard card in cards)
            {
                if(card.name == input)
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
            int i = 1;
            foreach (ICard card in cards)
            {                
                Console.WriteLine(i + ".) " + card.name);
                i++;
            }
        }

        public void addCardToStack(ICard card)
        {
            cards.Add(card);
        }
    }
}
