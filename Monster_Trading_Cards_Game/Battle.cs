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

        public Battle(User fighter1, User fighter2, int roundCounter, int winner)
        {
            this.fighter1 = fighter1;
            this.fighter2 = fighter2;
            this.roundCounter = roundCounter;
            this.winner = winner;
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
                else if(roundCounter >= 100)
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
            switch(monsterImmune(card1, card2))
            {
                case 0:
                    battle(card1, card2, card1.damage, card2.damage);
                    break;
                case 1:
                    battle(card1, card2, card1.damage, 0);
                    break;
                case 2:
                    battle(card1, card2, 0, card2.damage);
                    break;
                default:
                    break;
            }
        }

        public void spellFight(ICard card1, ICard card2)
        {
            int winningType = spellImmune(card1, card2);
            if(winningType != 0)
            {
                switch (winningType)
                {
                    case 1:
                        battle(card1, card2, card1.damage, 0);
                        break;
                    case 2:
                        battle(card1, card2, 0, card2.damage);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (elementComparison(card1, card2))
                {
                    case 0:
                        battle(card1, card2, card1.damage, card2.damage);
                        break;
                    case 1:
                        battle(card1, card2, card1.damage * 2, card2.damage / 2);
                        break;
                    case 2:
                        battle(card1, card2, card1.damage / 2, card2.damage * 2);
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

        public int spellImmune(ICard card1, ICard card2)
        {
            int winningType = 0;
            if (card1.monsterType == ICard.Monster_type.Knight && card2.elementType == ICard.Element_type.water && card2.monsterType == ICard.Monster_type.Spell)
            {
                winningType = 2;
            }
            else if (card1.monsterType == ICard.Monster_type.Spell && card1.elementType == ICard.Element_type.water && card2.monsterType == ICard.Monster_type.Knight)
            {
                winningType = 1;
            }
            else if (card1.monsterType == ICard.Monster_type.Kraken && card2.monsterType == ICard.Monster_type.Spell)
            {
                winningType = 1;
            }
            else if (card1.monsterType == ICard.Monster_type.Spell && card2.monsterType == ICard.Monster_type.Kraken)
            {
                winningType = 2;
            }
            return winningType;
        }
        public int monsterImmune(ICard card1, ICard card2)
        {
            int winningType = 0;
            if (card1.monsterType == ICard.Monster_type.Goblin && card2.monsterType == ICard.Monster_type.Dragon)
            {
                winningType = 2;
            }
            else if (card1.monsterType == ICard.Monster_type.Dragon && card2.monsterType == ICard.Monster_type.Goblin)
            {
                winningType = 1;
            }
            else if (card1.monsterType == ICard.Monster_type.Wizzard && card2.monsterType == ICard.Monster_type.Ork)
            {
                winningType = 1;
            }
            else if (card1.monsterType == ICard.Monster_type.Ork && card2.monsterType == ICard.Monster_type.Wizzard)
            {
                winningType = 2;
            }
            else if (card1.monsterType == ICard.Monster_type.Elve && card1.elementType == ICard.Element_type.fire && card2.monsterType == ICard.Monster_type.Dragon)
            {
                winningType = 1;
            }
            else if (card1.monsterType == ICard.Monster_type.Dragon && card2.elementType == ICard.Element_type.fire && card2.monsterType == ICard.Monster_type.Elve)
            {
                winningType = 2;
            }
            return winningType;
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
                    fighter1.elo += 3;

                    fighter2.playedGames++;
                    fighter2.elo -= 5;
                    break;
                case 2:
                    fighter2.playedGames++;
                    fighter2.wonGames++;
                    fighter2.elo += 3;

                    fighter1.playedGames++;
                    fighter1.elo -= 5;
                    break;
                default:
                    break;
            }
        }
    }
}
