using NUnit.Framework;
using System;
using Monster_Trading_Cards_Game;
using Monster_Trading_Cards_Game.Cards;
using System.Collections.Generic;

namespace Monster_Trading_Cards_Game.Test
{
    public class BattleTest
    {
        public Battle battle;
        public SpellCard waterSpell, fireSpell;
        public MonsterCard fireElve, fireDragon, waterGoblin, normalKnight, fireWizzard, waterKraken, normalOrk;

        [SetUp]
        public void Setup()
        {
            waterSpell = new SpellCard(1, "WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Kraken);
            fireSpell = new SpellCard(1, "FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Kraken);

            fireElve = new MonsterCard(1, "FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None);
            fireDragon = new MonsterCard(1, "FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon, ICard.Monster_type.Elve);
            waterGoblin = new MonsterCard(1, "WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon);
            normalKnight = new MonsterCard(1, "NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight, ICard.Monster_type.Spell);
            fireWizzard = new MonsterCard(1, "FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard, ICard.Monster_type.None);
            waterKraken = new MonsterCard(1, "WaterKraken", 15, ICard.Element_type.water, ICard.Monster_type.Kraken, ICard.Monster_type.None);
            normalOrk = new MonsterCard(1, "NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard);

            List <ICard> cards= new List<ICard>();
            cards.Add(normalOrk);
            CardDeck deck = new CardDeck(cards);
            User fighter1 = new User(1, "Fighter1", "123", 20, 100, 0, 0, null, deck);
            User fighter2 = new User(2, "Fighter2", "123", 20, 100, 0, 0, null, deck);

            battle = new Battle(fighter1, fighter2);
        }

        [Test]
        public void elementComparison_waterGoblin_fireDragon()
        {
            //act
            int strongerElement = battle.elementComparison(waterGoblin, fireDragon);
            //assert
            Assert.AreEqual(1, strongerElement);
        }

        [Test]
        public void elementComparison_fireDragon_waterGoblin()
        {
            //act
            int strongerElement = battle.elementComparison(fireDragon, waterGoblin);
            //assert
            Assert.AreEqual(2, strongerElement);
        }

        [Test]
        public void elementComparison_fireDragon_normalKnight()
        {
            //act
            int strongerElement = battle.elementComparison(fireDragon, normalKnight);
            //assert
            Assert.AreEqual(1, strongerElement);
        }

        [Test]
        public void elementComparison_normalKnight_fireDragon()
        {
            //act
            int strongerElement = battle.elementComparison(normalKnight, fireDragon);
            //assert
            Assert.AreEqual(2, strongerElement);
        }

        [Test]
        public void elementComparison_normalKnight_waterGoblin()
        {
            //act
            int strongerElement = battle.elementComparison(normalKnight, waterGoblin);
            //assert
            Assert.AreEqual(1, strongerElement);
        }

        [Test]
        public void elementComparison_waterGoblin_normalKnight()
        {
            //act
            int strongerElement = battle.elementComparison(waterGoblin, normalKnight);
            //assert
            Assert.AreEqual(2, strongerElement);
        }
    }
}