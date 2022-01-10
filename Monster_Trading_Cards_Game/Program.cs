using Monster_Trading_Cards_Game.Database;
using Monster_Trading_Cards_Game.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;

namespace Monster_Trading_Cards_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            //Upload Cards to Database
            //UploadCardsToDB cardUpload = new UploadCardsToDB();
            //cardUpload.upload();

            //DB Test
            DB database = new DB();
            database.Connect();

            CardDeck deck1 = new CardDeck();
            deck1.addCardInBattle(database.getCardByID(1));
            deck1.addCardInBattle(database.getCardByID(2));
            deck1.addCardInBattle(database.getCardByID(3));
            deck1.addCardInBattle(database.getCardByID(4));
            deck1.addCardInBattle(database.getCardByID(5));

            CardDeck deck2 = new CardDeck();
            deck2.addCardInBattle(database.getCardByID(5));
            deck2.addCardInBattle(database.getCardByID(6));
            deck2.addCardInBattle(database.getCardByID(7));
            deck2.addCardInBattle(database.getCardByID(8));
            deck2.addCardInBattle(database.getCardByID(9));

            //TestStack
            CardStack stack = new CardStack(new List<ICard>{
            database.getCardByID(1),
            database.getCardByID(2),
            database.getCardByID(3),
            database.getCardByID(4),
            database.getCardByID(5),
            database.getCardByID(6),
            database.getCardByID(7),
            database.getCardByID(8),
            database.getCardByID(9)
            });

            User newUser1 = new User(0, "TestUser", "PW123", 20, 100, 20, 100, stack, deck1);
            User newUser2 = new User(0, "Userer", "PW123", 20, 100, 20, 100, stack, deck2);

            //Actual Program
            int tempInput = 0;
            int input = -1;
            ConsoleNavigation navigation = new ConsoleNavigation();
            User loggedInUser = null;
            while(input == -1)
            {
                Console.Clear();
                Console.CursorVisible = false;
                Console.WriteLine("[ ] Register");
                Console.WriteLine("[ ] Login as existing User");
                input = navigation.moveCursor(2, Console.CursorTop);
            }
            Console.Clear();
            Console.CursorVisible = true;
            if (input == 1)
            {
                while (true)
                {
                    Console.WriteLine("Enter Username");
                    string username = Console.ReadLine();
                    if (!database.userExists(username))
                    {
                        Console.WriteLine("Enter Password");
                        string password = Console.ReadLine();
                        database.createUser(username, password);
                        loggedInUser = database.loginUser(username);
                        break;
                    }
                }
            }
            else
            {
                do
                {
                    Console.WriteLine("Enter Username");
                    string username = Console.ReadLine();
                    Console.WriteLine("Enter Password");
                    string password = Console.ReadLine();
                    if (database.userExists(username))
                    {
                        if (database.getUserPW(username) == password)
                        {
                            loggedInUser = database.loginUser(username);
                        }
                        else
                        {
                            Console.WriteLine("Wrong Password!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("User doesn't exist!");
                    }

                } while (loggedInUser == null);
            }
            do
            {
                Console.CursorVisible = false;
                input = -1;
                while(input == -1)
                {
                    Console.Clear();
                    Console.WriteLine("[ ] Edit Deck");
                    Console.WriteLine("[ ] Fight");
                    Console.WriteLine("[ ] Buy Pack");
                    Console.WriteLine("[ ] Profile");
                    Console.WriteLine("[ ] Scoreboard");
                    Console.WriteLine("[ ] Trade Card");
                    Console.WriteLine("[ ] Edit active Trades");
                    Console.WriteLine("[ ] Watch Trade Offers");
                    Console.WriteLine("[ ] Exit");
                    input = navigation.moveCursor(9, Console.CursorTop);
                }
                switch (input)
                {
                    case 1:
                        loggedInUser.setDeck();
                        break;
                    case 2:
                        if(loggedInUser.deck.cardCount() >= 4)
                        {
                            Battle battle = new Battle(loggedInUser, newUser2);
                            battle.fight();

                            database.updateUserStats(loggedInUser.id, loggedInUser.coins, loggedInUser.elo, loggedInUser.playedGames, loggedInUser.wonGames);
                        }
                        else
                        {
                            Console.WriteLine("Your deck contains less than 4 cards!");
                            Console.Write("Press any Key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        if(loggedInUser.coins < 5)
                        {
                            Console.WriteLine("You need 5 coins to buy a Pack!");
                            Console.WriteLine("You currently have " + loggedInUser.coins + " coins!");
                        }
                        else
                        {
                            tempInput = -1;
                            while(tempInput == -1)
                            {
                                Console.Clear();
                                Console.WriteLine("Do you want a Pack with 5 cards for 5 coins?");
                                Console.WriteLine("[ ] Yes");
                                Console.WriteLine("[ ] No");
                                tempInput = navigation.moveCursor(2, Console.CursorTop);
                            }
                            
                            switch (tempInput)
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
                        break;
                    case 4:
                        tempInput = -1;
                        while (tempInput == -1)
                        {
                            Console.Clear();
                            loggedInUser.printProfile();
                            Console.WriteLine("[ ] Return to menu");
                            Console.WriteLine("[ ] Change password");
                            tempInput = navigation.moveCursor(2, Console.CursorTop);
                        }
                        Console.Clear();
                        switch (tempInput)
                        {
                            case 2:
                                Console.CursorVisible = true;
                                Console.WriteLine("Enter password:");
                                string password = Console.ReadLine();
                                database.updatePassword(loggedInUser.id, password);
                                loggedInUser.password = password;
                                break;
                            default:
                                break;
                        }
                        break;
                    case 5:
                        database.printScoreboard();
                        Console.Write("Press any Key to continue...");
                        Console.ReadLine();
                        break;
                    case 6:
                        while (true)
                        {
                            tempInput = -1;
                            while (tempInput == -1)
                            {
                                Console.Clear();
                                Console.WriteLine("Choose Card to trade");
                                loggedInUser.stack.printList();
                                tempInput = navigation.moveCursor(loggedInUser.stack.cards.Count, Console.CursorTop);
                            }
                            Console.Clear();
                            if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[tempInput-1]))
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
                                if(wantedType != 1)
                                {
                                    wantedType = 0;
                                }
                                database.tradeCard(loggedInUser.stack.cards[tempInput-1].id, loggedInUser.id, wantedDamage, wantedType);
                                loggedInUser.stack.cards.Remove(loggedInUser.stack.cards[tempInput-1]);
                                break;
                            }
                        }
                        break;
                    case 7:
                        List<int> cardIDs = database.activeTradesCardIDs(loggedInUser.id);
                        string printList = database.activeTradesPrint(loggedInUser.id);
                        string chooseList = database.activeTradesChoose(loggedInUser.id);
                        tempInput = -1;
                        while (tempInput == -1)
                        {
                            Console.Clear();
                            Console.Write(printList);
                            Console.WriteLine("[ ] Return to menu");
                            Console.WriteLine("[ ] Cancel trade offer");
                            tempInput = navigation.moveCursor(2, Console.CursorTop);
                        }
                        Console.Clear();
                        if (tempInput == 2)
                        {
                            tempInput = -1;
                            while (tempInput == -1)
                            {
                                Console.Clear();
                                Console.Write(chooseList);
                                tempInput = navigation.moveCursor(cardIDs.Count, Console.CursorTop);
                            }
                            int cardID = cardIDs[tempInput-1];
                            if (cardIDs.Contains(cardID)){
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
                        break;
                    case 8:
                        List<int> tradeIDs = database.allTradeIDs(loggedInUser.id);
                        if(tradeIDs.Count <= 0)
                        {
                            break;
                        }
                        string print = database.printAllTrades(loggedInUser.id);
                        tempInput = -1;
                        while (tempInput == -1)
                        {
                            Console.Clear();
                            Console.Write(print);
                            tempInput = navigation.moveCursor(tradeIDs.Count, Console.CursorTop);
                        }
                        Console.Clear();
                        int tradeID = tradeIDs[tempInput - 1];
                        if (tradeIDs.Contains(tradeID))
                        {
                            tempInput = -1;
                            while (tempInput == -1)
                            {
                                Console.Clear();
                                Console.WriteLine("What card do you want to trade?");
                                loggedInUser.stack.printList();
                                tempInput = navigation.moveCursor(loggedInUser.stack.cards.Count, Console.CursorTop);
                            }
                            Console.Clear();
                            if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[tempInput-1]))
                            {
                                //trade -> wants spell?
                                int[] wanteds = database.tradeWanteds(tradeID);
                                if(loggedInUser.stack.cards[tempInput - 1].damage <= wanteds[0])
                                {
                                    Console.WriteLine("Card hasn't enough damage!");
                                    Console.Write("Press any Key to continue...");
                                    Console.ReadLine();
                                    break;
                                }
                                else if((loggedInUser.stack.cards[tempInput - 1].monsterType == ICard.Monster_type.Spell && wanteds[1] != 1) || (loggedInUser.stack.cards[tempInput - 1].monsterType != ICard.Monster_type.Spell && wanteds[1] == 1))
                                {
                                    Console.WriteLine("Card hasn't the wanted Type!");
                                    Console.Write("Press any Key to continue...");
                                    Console.ReadLine();
                                    break;
                                }
                                else
                                {
                                    int[] ids = database.acceptTradeOffer(tradeID, loggedInUser.id);
                                    loggedInUser.stack.addCardToStack(database.getCardByID(ids[0]));
                                    database.returnCardForTrade(ids[1], tempInput, loggedInUser.id);
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
                        break;
                    default:
                        break;
                }
            } while (input != 9);
            database.Disconnect();
        }
    }
}
//TODO
//Token
//Unique feature
//Gegnersuche

//Deck erstellen Console
//Battle Farben

//Trading damage werte usw anzeigen
//Deck Creator verbessern

//Register und Login Nullabfragen

//Battle Logic alle ausnahmefälle einbinden
//evtl neue Karten erstellen
//Class Diagram
//DB Singleton