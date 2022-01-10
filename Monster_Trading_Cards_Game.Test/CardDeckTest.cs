using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Monster_Trading_Cards_Game;
using Monster_Trading_Cards_Game.Cards;

namespace Monster_Trading_Cards_Game.Test
{
    class CardDeckTest
    {
        public CardDeck deck1, deck2;
        public MonsterCard fireElve;
        [SetUp]
        public void Setup()
        {
            MonsterCard normalOrk = new MonsterCard(1, "NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);
            fireElve = new MonsterCard(1, "FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None);
            List<ICard> cards1 = new List<ICard>();
            List<ICard> cards2 = new List<ICard>();
            cards1.Add(normalOrk);
            cards2.Add(normalOrk);
            cards2.Add(fireElve);
            deck1 = new CardDeck(cards1);
            deck2 = new CardDeck(cards2);
        }

        [Test]
        public void cardCount_Equals_2()
        {
            //act
            int cardCount = deck2.cardCount();
            //assert
            Assert.AreEqual(2, cardCount);
        }

        [Test]
        public void addCardInBattle_returns_true()
        {
            //act
            bool isNotInDeck = deck1.addCardInBattle(fireElve);
            //assert
            Assert.AreEqual(true, isNotInDeck);
        }

        [Test]
        public void addCardInBattle_returns_false()
        {
            //act
            bool isNotInDeck = deck2.addCardInBattle(fireElve);
            //assert
            Assert.AreEqual(false, isNotInDeck);
        }
    }
}
