using System;
using System.Collections.Generic;

namespace Monster_Trading_Cards_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestStack
            CardStack stack = new CardStack(new List<ICard>
            {
                //CardCreator
                //Spells
                new SpellCard("FireSpell", 10, ICard.Element_type.fire),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water),

                //Monsters
                new MonsterCard("FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon),
                new MonsterCard("WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin),
                new MonsterCard("FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve),
                new MonsterCard("NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight),
                new MonsterCard("FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard),
                new MonsterCard("WaterKraken", 15, ICard.Element_type.water, ICard.Monster_type.Kraken),
                new MonsterCard("NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork)
            });

            CardDeck deck1 = new CardDeck(new List<ICard>
            {
                new MonsterCard("FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon),
                new MonsterCard("WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin),
                new MonsterCard("FireElve", 10, ICard.Element_type.fire, ICard.Monster_type.Elve),
                new MonsterCard("NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight)
            });
            CardDeck deck2 = new CardDeck(new List<ICard>
            {
                new MonsterCard("FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard),
                new MonsterCard("WaterKraken", 5, ICard.Element_type.water, ICard.Monster_type.Kraken),
                new MonsterCard("NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water)

            });
            CardDeck spelldeck = new CardDeck(new List<ICard>
            {
                new SpellCard("FireSpell", 10, ICard.Element_type.fire),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water),
                new SpellCard("FireSpell", 10, ICard.Element_type.fire),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water)

            });
            User newUser1 = new User("TestUser", "PW123", stack, deck1);
            User newUser2 = new User("Userer", "PW123", stack, spelldeck);

            //Actual Program
            int input;
            do
            {
                Console.Clear();
                Console.WriteLine("1.) Edit Deck");
                Console.WriteLine("2.) Fight");
                Console.WriteLine("5.) Exit");
                input = int.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        newUser1.setDeck();
                        break;
                    case 2:
                        Battle battle = new Battle(newUser1, newUser2, 0, 0);
                        battle.fight();
                        break;
                    default:
                        break;
                }
            } while (input != 5);
        }
    }
}
//TODO
//Unit Testing