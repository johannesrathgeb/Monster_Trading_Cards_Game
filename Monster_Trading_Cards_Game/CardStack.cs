using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class CardStack
    {
        List<ICard> cards = new List<ICard>();

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
        public void printList()
        {
            foreach (ICard card in cards)
            {
                Console.WriteLine(card.name);
            }
        }
    }
}
