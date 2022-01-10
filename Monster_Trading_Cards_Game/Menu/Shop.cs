using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Menu
{
    static class Shop
    {
        public static void packs(User loggedInUser)
        {
            int input;
            DB database = DB.getInstance();
            ConsoleNavigation navigation = new ConsoleNavigation();
            if (loggedInUser.coins < 5)
            {
                Console.WriteLine("You need 5 coins to buy a Pack!");
                Console.WriteLine("You currently have " + loggedInUser.coins + " coins!");
            }
            else
            {
                input = -1;
                while (input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("Do you want a Pack with 5 cards for 5 coins?");
                    Console.WriteLine("[ ] Yes");
                    Console.WriteLine("[ ] No");
                    input = navigation.moveCursor(2, Console.CursorTop);
                }

                switch (input)
                {
                    case 1:
                        loggedInUser.coins -= 5;
                        database.updateUserStats(loggedInUser.id, loggedInUser.coins, loggedInUser.elo, loggedInUser.playedGames, loggedInUser.wonGames);
                        for (int i = 0; i <= 4; i++)
                        {
                            Console.Clear();
                            List<ICard> cardList = database.getAllCards();
                            var random = new Random();
                            int index = random.Next(cardList.Count);
                            database.addCardToStack(cardList[index].id, loggedInUser.id);
                            loggedInUser.stack.addCardToStack(cardList[index]);
                            Console.WriteLine("You got " + cardList[index].name + " Damage: " + cardList[index].damage + "!!!");
                            Console.Write("Press any Key to continue...");
                            Console.ReadLine();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static void ownCard(User loggedInUser)
        {
            int input;
            DB database = DB.getInstance();
            ConsoleNavigation navigation = new ConsoleNavigation();
            if (loggedInUser.coins < 10)
            {
                Console.Clear();
                Console.WriteLine("You need 10 coins to create a Card!");
                Console.WriteLine("You currently have " + loggedInUser.coins + " coins!");
                Console.Write("Press any Key to continue...");
                Console.ReadLine();
            }
            else
            {
                input = -1;
                while (input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("Do you want to create a Card for 10 coins?");
                    Console.WriteLine("[ ] Yes");
                    Console.WriteLine("[ ] No");
                    input = navigation.moveCursor(2, Console.CursorTop);
                }

                switch (input)
                {
                    case 1:
                        loggedInUser.coins -= 10;
                        database.updateUserStats(loggedInUser.id, loggedInUser.coins, loggedInUser.elo, loggedInUser.playedGames, loggedInUser.wonGames);
                        Console.Clear();
                        Console.CursorVisible = true;
                        Console.WriteLine("What should your card be called?");
                        string cardname = Console.ReadLine();
                        int carddamage = -1;
                        while (carddamage < 1 || carddamage > 20)
                        {
                            Console.Clear();
                            Console.WriteLine("How much damage should it have? (1-20)");
                            carddamage = int.Parse(Console.ReadLine());
                        }
                        Console.CursorVisible = false;
                        ICard.Element_type element;
                        int elementChoose = -1;
                        while (elementChoose == -1)
                        {
                            Console.Clear();
                            Console.WriteLine("Choose Element Type");
                            Console.WriteLine("[ ] Fire");
                            Console.WriteLine("[ ] Water");
                            Console.WriteLine("[ ] Normal");
                            elementChoose = navigation.moveCursor(3, Console.CursorTop);
                        }
                        element = (ICard.Element_type)elementChoose;
                        ICard.Monster_type monstertype;
                        int monsterchoose = -1;
                        while (monsterchoose == -1)
                        {
                            Console.Clear();
                            Console.WriteLine("Choose Monster Type");
                            Console.WriteLine("[ ] Goblin");
                            Console.WriteLine("[ ] Dragon");
                            Console.WriteLine("[ ] Wizzard");
                            Console.WriteLine("[ ] Knight");
                            Console.WriteLine("[ ] Kraken");
                            Console.WriteLine("[ ] Elve");
                            Console.WriteLine("[ ] Ork");
                            Console.WriteLine("[ ] Spell");
                            monsterchoose = navigation.moveCursor(8, Console.CursorTop);
                        }
                        monstertype = (ICard.Monster_type)monsterchoose;
                        ICard.Monster_type weakness;
                        int weaknesschoose = -1;
                        while (weaknesschoose == -1)
                        {
                            Console.Clear();
                            Console.WriteLine("Choose weakness");
                            Console.WriteLine("[ ] Goblin");
                            Console.WriteLine("[ ] Dragon");
                            Console.WriteLine("[ ] Wizzard");
                            Console.WriteLine("[ ] Knight");
                            Console.WriteLine("[ ] Kraken");
                            Console.WriteLine("[ ] Elve");
                            Console.WriteLine("[ ] Ork");
                            Console.WriteLine("[ ] Spell");
                            weaknesschoose = navigation.moveCursor(8, Console.CursorTop);
                        }
                        weakness = (ICard.Monster_type)weaknesschoose;
                        int id = (database.getHighestCardID() + 1);
                        ICard customCard = null;
                        if (monstertype == ICard.Monster_type.Spell)
                        {
                            customCard = new SpellCard(id, cardname, carddamage, element, weakness);
                        }
                        else
                        {
                            customCard = new MonsterCard(id, cardname, carddamage, element, monstertype, weakness);
                        }
                        database.createCard(cardname, carddamage, element, monstertype, weakness);
                        database.addCardToStack(customCard.id, loggedInUser.id);
                        loggedInUser.stack.addCardToStack(customCard);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    
}
