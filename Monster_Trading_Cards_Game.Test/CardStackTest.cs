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
    class CardStackTest
    {
        CardStack stack;
        MonsterCard fireElve;
        [SetUp]
        public void Setup()
        {
            MonsterCard normalOrk = new MonsterCard(1, "NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);
            fireElve = new MonsterCard(2, "FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None);
            List<ICard> cards1 = new List<ICard>();
            cards1.Add(normalOrk);
            cards1.Add(fireElve);
            stack = new CardStack(cards1);
        }

        [Test]
        public void isCardInStack_returns_true()
        {
            //act
            bool isInStack = stack.isCardInStack(2);
            //assert
            Assert.AreEqual(true, isInStack);
        }

        [Test]
        public void isCardInStack_returns_false()
        {
            //act
            bool isInStack = stack.isCardInStack(3);
            //assert
            Assert.AreEqual(false, isInStack);
        }

        [Test]
        public void searchCard_Exists()
        {
            //act
            ICard foundCard = stack.searchCard("FireElve");
            //assert
            Assert.AreEqual(fireElve, foundCard);
        }

        [Test]
        public void searchCard_doesnt_Exist()
        {
            //act
            ICard foundCard = stack.searchCard("RandomCard");
            //assert
            Assert.AreEqual(null, foundCard);
        }
    }
}
