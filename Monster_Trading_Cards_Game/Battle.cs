using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monster_Trading_Cards_Game
{
    class Battle
    {
        User fighter1, fighter2;
        int roundCounter, winner;
        ICard card1, card2;
        int zeroDamage = 0;
        int damageMultiplier = 2;
        int damageDivisor = 2;
        int winnerElo = 3;
        int loserElo = 5;
        int maxRounds = 100;

        public Battle(User fighter1, User fighter2)
        {
            this.fighter1 = fighter1;
            this.fighter2 = fighter2;
            roundCounter = 0;
            winner = 0;
        }

        public void fight()
        {
            do
            {
                card1 = fighter1.deck.randomCard();
                card2 = fighter2.deck.randomCard();

                Console.WriteLine();
                Console.WriteLine("Next Round!");
                Console.WriteLine(card1.name + " of type " + card1.monsterType + " and of element " + card1.elementType + " with default damage of " + card1.damage + "!");
                Console.WriteLine("VS.");
                Console.WriteLine(card2.name + " of type " + card2.monsterType + " and of element " + card2.elementType + " with default damage of " + card2.damage + "!");


                if (card1.monsterType == ICard.Monster_type.Spell || card2.monsterType == ICard.Monster_type.Spell)
                {
                    spellFight(card1, card2);
                }
                else
                {
                    monsterFight(card1, card2);
                }

                //Endings
                if(fighter1.deck.cardCount() <= 0)
                {
                    winner = 2;
                    Console.WriteLine("Player 2 wins!");
                    break;
                }
                else if(fighter2.deck.cardCount() <= 0)
                {
                    winner = 1;
                    Console.WriteLine("Player 1 wins!");
                    break;
                }
                else if(roundCounter >= maxRounds)
                {
                    Console.WriteLine("Draw!");
                    break;
                }
                else
                {
                    roundCounter++;
                }
            } while (true);
            Console.WriteLine("Beliebige Taste drücken...");
            Console.ReadKey();
            updateUserStats(winner);
        }

        public void monsterFight(ICard card1, ICard card2)
        {
            if(card1.weakness == card2.monsterType)
            {
                battle(card1, card2, zeroDamage, card2.damage);
            }
            else if(card2.weakness == card1.monsterType)
            {
                battle(card1, card2, card1.damage, zeroDamage);
            }
            else
            {
                battle(card1, card2, card1.damage, card2.damage);
            }
        }

        public void spellFight(ICard card1, ICard card2)
        {
            if(card1.weakness == ICard.Monster_type.Spell)
            {
                battle(card1, card2, zeroDamage, card2.damage);
            }
            else if(card2.weakness == ICard.Monster_type.Spell)
            {
                battle(card1, card2, card1.damage, zeroDamage);
            }
            else
            {
                switch (elementComparison(card1, card2))
                {
                    case 0:
                        battle(card1, card2, card1.damage, card2.damage);
                        break;
                    case 1:
                        battle(card1, card2, card1.damage * damageMultiplier, card2.damage / damageDivisor);
                        break;
                    case 2:
                        battle(card1, card2, card1.damage / damageDivisor, card2.damage * damageMultiplier);
                        break;
                }
            }
        }

        public void battle(ICard card1, ICard card2, double damage1, double damage2)
        {
            if (damage1 > damage2)
            {
                Console.WriteLine("Fighter 1 wins with " + card1.name + "!");
                fighter1.deck.addCard(card2);
                fighter2.deck.removeCard(card2);
            }
            else if (damage1 < damage2)
            {
                Console.WriteLine("Fighter 2 wins " + card2.name + "!");
                fighter2.deck.addCard(card1);
                fighter1.deck.removeCard(card1);
            }
            else
            {
                Console.WriteLine("Draw!");
            }
        }

        public int elementComparison(ICard card1, ICard card2)
        {
            int strongerElement = 0;
            if(card1.elementType == ICard.Element_type.fire)
            {
                switch (card2.elementType)
                {
                    case ICard.Element_type.water:
                        strongerElement = 2;
                        break;
                    case ICard.Element_type.normal:
                        strongerElement = 1;
                        break;
                    default:
                        break;
                }
            }
            else if(card1.elementType == ICard.Element_type.water)
            {
                switch (card2.elementType)
                {
                    case ICard.Element_type.fire:
                        strongerElement = 1;
                        break;
                    case ICard.Element_type.normal:
                        strongerElement = 2;
                        break;
                    default:
                        break;
                }
            }
            else if (card1.elementType == ICard.Element_type.normal)
            {
                switch (card2.elementType)
                {
                    case ICard.Element_type.fire:
                        strongerElement = 2;
                        break;
                    case ICard.Element_type.water:
                        strongerElement = 1;
                        break;
                    default:
                        break;
                }
            }
            return strongerElement;
        }

        public void updateUserStats(int winner)
        {
            switch (winner)
            {
                case 0:
                    fighter1.playedGames++;
                    fighter2.playedGames++;
                    break;
                case 1:
                    fighter1.playedGames++;
                    fighter1.wonGames++;
                    fighter1.elo += winnerElo;

                    fighter2.playedGames++;
                    fighter2.elo -= loserElo;
                    break;
                case 2:
                    fighter2.playedGames++;
                    fighter2.wonGames++;
                    fighter2.elo += winnerElo;

                    fighter1.playedGames++;
                    fighter1.elo -= loserElo;
                    break;
                default:
                    break;
            }
        }
    }
}
