using Monster_Trading_Cards_Game.Database;
using System;
using System.Collections.Generic;

namespace Monster_Trading_Cards_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            //DB Test
            DB database = new DB();
            database.Connect();
            //database.commandtest();
            database.outputTest();
            database.Disconnect();

            //TestStack
            CardStack stack = new CardStack(new List<ICard>
            {
                //CardCreator
                //Spells
                new SpellCard("FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Kraken),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Kraken),

                //Monsters
                new MonsterCard("FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon, ICard.Monster_type.Elve),
                new MonsterCard("WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon),
                new MonsterCard("FireElve", 5, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None),
                new MonsterCard("NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight, ICard.Monster_type.Spell),
                new MonsterCard("FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard, ICard.Monster_type.None),
                new MonsterCard("WaterKraken", 15, ICard.Element_type.water, ICard.Monster_type.Kraken, ICard.Monster_type.None),
                new MonsterCard("NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard)
            });

            CardDeck deck1 = new CardDeck(new List<ICard>
            {
                new MonsterCard("FireDragon", 15, ICard.Element_type.fire, ICard.Monster_type.Dragon, ICard.Monster_type.Elve),
                new MonsterCard("WaterGoblin", 10, ICard.Element_type.water, ICard.Monster_type.Goblin, ICard.Monster_type.Dragon),
                new MonsterCard("FireElve", 10, ICard.Element_type.fire, ICard.Monster_type.Elve, ICard.Monster_type.None),
                new MonsterCard("NormalKnight", 10, ICard.Element_type.normal, ICard.Monster_type.Knight, ICard.Monster_type.Spell)
            });
            CardDeck deck2 = new CardDeck(new List<ICard>
            {
                new MonsterCard("FireWizzard", 10, ICard.Element_type.fire, ICard.Monster_type.Wizzard, ICard.Monster_type.None),
                new MonsterCard("WaterKraken", 5, ICard.Element_type.water, ICard.Monster_type.Kraken, ICard.Monster_type.None),
                new MonsterCard("NormalOrk", 5, ICard.Element_type.normal, ICard.Monster_type.Ork, ICard.Monster_type.Wizzard),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Kraken)

            });
            CardDeck spelldeck = new CardDeck(new List<ICard>
            {
                new SpellCard("FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Kraken),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Kraken),
                new SpellCard("FireSpell", 10, ICard.Element_type.fire, ICard.Monster_type.Kraken),
                new SpellCard("WaterSpell", 10, ICard.Element_type.water, ICard.Monster_type.Kraken)

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
                        Battle battle = new Battle(newUser1, newUser2);
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
//Battle Logic alle ausnahmefälle einbinden
//Unit Testing
//Enums auslagern?
//MagicNumbers
//WriteLine mit $
//evtl neue Karten erstellen
//Class Diagram