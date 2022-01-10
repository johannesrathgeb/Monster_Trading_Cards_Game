using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Menu
{
    static class Trading
    {
        public static void tradeCard(User loggedInUser)
        {
            int input;
            DB database = DB.getInstance();
            ConsoleNavigation navigation = new ConsoleNavigation();
            while (true)
            {
                input = -1;
                while (input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("Choose Card to trade");
                    loggedInUser.stack.printList();
                    input = navigation.moveCursor(loggedInUser.stack.cards.Count, Console.CursorTop);
                }
                Console.Clear();
                if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[input - 1]))
                {
                    Console.WriteLine("Whats your minimun damage requirement to trade?");
                    Console.CursorVisible = true;
                    int wantedDamage = int.Parse(Console.ReadLine());
                    Console.CursorVisible = false;
                    Console.Clear();

                    int wantedType = -1;
                    while (wantedType == -1)
                    {
                        Console.Clear();
                        Console.WriteLine("What card type do you want to trade for?");
                        Console.WriteLine("[ ] Spell");
                        Console.WriteLine("[ ] Monster");
                        wantedType = navigation.moveCursor(2, Console.CursorTop);
                    }
                    if (wantedType != 1)
                    {
                        wantedType = 0;
                    }
                    database.tradeCard(loggedInUser.stack.cards[input - 1].id, loggedInUser.id, wantedDamage, wantedType);
                    loggedInUser.stack.cards.Remove(loggedInUser.stack.cards[input - 1]);
                    break;
                }
            }
        }

        public static void editTrades(User loggedInUser)
        {
            int input;
            DB database = DB.getInstance();
            ConsoleNavigation navigation = new ConsoleNavigation();
            List<int> cardIDs = database.activeTradesCardIDs(loggedInUser.id);
            string printList = database.activeTradesPrint(loggedInUser.id);
            string chooseList = database.activeTradesChoose(loggedInUser.id);
            input = -1;
            while (input == -1)
            {
                Console.Clear();
                Console.Write(printList);
                Console.WriteLine("[ ] Return to menu");
                Console.WriteLine("[ ] Cancel trade offer");
                input = navigation.moveCursor(2, Console.CursorTop);
            }
            Console.Clear();
            if (input == 2)
            {
                input = -1;
                while (input == -1)
                {
                    Console.Clear();
                    Console.Write(chooseList);
                    input = navigation.moveCursor(cardIDs.Count, Console.CursorTop);
                }
                int cardID = cardIDs[input - 1];
                if (cardIDs.Contains(cardID))
                {
                    database.cancelTrade(cardID, loggedInUser.id);
                    loggedInUser.stack.addCardToStack(database.getCardByID(cardID));
                }
                else
                {
                    Console.WriteLine("Card not found!");
                    Console.Write("Press any Key to continue...");
                    Console.ReadLine();
                }
            }
        }

        public static void watchTrades(User loggedInUser)
        {
            int input;
            DB database = DB.getInstance();
            ConsoleNavigation navigation = new ConsoleNavigation();
            List<int> tradeIDs = database.allTradeIDs(loggedInUser.id);
            if (tradeIDs.Count <= 0)
            {
                return;
            }
            string print = database.printAllTrades(loggedInUser.id);
            input = -1;
            while (input == -1)
            {
                Console.Clear();
                Console.Write(print);
                input = navigation.moveCursor(tradeIDs.Count, Console.CursorTop);
            }
            Console.Clear();
            int tradeID = tradeIDs[input - 1];
            if (tradeIDs.Contains(tradeID))
            {
                input = -1;
                while (input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("What card do you want to trade?");
                    loggedInUser.stack.printList();
                    input = navigation.moveCursor(loggedInUser.stack.cards.Count, Console.CursorTop);
                }
                Console.Clear();
                if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[input - 1]))
                {
                    //trade -> wants spell?
                    int[] wanteds = database.tradeWanteds(tradeID);
                    if (loggedInUser.stack.cards[input - 1].damage <= wanteds[0])
                    {
                        Console.WriteLine("Card hasn't enough damage!");
                        Console.Write("Press any Key to continue...");
                        Console.ReadLine();
                        return;
                    }
                    else if ((loggedInUser.stack.cards[input - 1].monsterType == ICard.Monster_type.Spell && wanteds[1] != 1) || (loggedInUser.stack.cards[input - 1].monsterType != ICard.Monster_type.Spell && wanteds[1] == 1))
                    {
                        Console.WriteLine("Card hasn't the wanted Type!");
                        Console.Write("Press any Key to continue...");
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        int[] ids = database.acceptTradeOffer(tradeID, loggedInUser.id);
                        loggedInUser.stack.addCardToStack(database.getCardByID(ids[0]));
                        database.returnCardForTrade(ids[1], input, loggedInUser.id);
                    }
                }
                else
                {
                    Console.WriteLine("Card is in your Deck!");
                    Console.Write("Press any Key to continue...");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Trade not found!");
                Console.Write("Press any Key to continue...");
                Console.ReadLine();
            }
        }
    }
}
