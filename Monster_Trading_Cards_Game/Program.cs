using Monster_Trading_Cards_Game.Database;
using Monster_Trading_Cards_Game.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            int input = 0;
            User loggedInUser = null;
            while(input != 1 && input != 2)
            {
                Console.WriteLine("1.) Register as new User");
                Console.WriteLine("2.) Login as existing User");
                input = int.Parse(Console.ReadLine());
            }
            if(input == 1)
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
                    if (database.getUserPW(username) == password)
                    {
                        loggedInUser = database.loginUser(username);
                    }
                } while (loggedInUser == null);
            }
            do
            {
                Console.Clear();
                Console.WriteLine("1.) Edit Deck");
                Console.WriteLine("2.) Fight");
                Console.WriteLine("3.) Buy Pack");
                Console.WriteLine("4.) Profile");
                Console.WriteLine("5.) Scoreboard");
                Console.WriteLine("6.) Trade Card");
                Console.WriteLine("7.) Edit active Trades");
                Console.WriteLine("8.) Watch Trade Offers");
                Console.WriteLine("9.) Exit");
                input = int.Parse(Console.ReadLine());
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
                            Console.WriteLine("You don't have enough coins!");
                        }
                        else
                        {
                            Console.WriteLine("Do you want a Pack with 5 cards for 5 coins?");
                            Console.WriteLine("1.) Yes");
                            Console.WriteLine("2.) No");
                            tempInput = int.Parse(Console.ReadLine());
                            switch (tempInput)
                            {
                                case 1:
                                    loggedInUser.coins -= 5;
                                    database.updateUserStats(loggedInUser.id, loggedInUser.coins, loggedInUser.elo, loggedInUser.playedGames, loggedInUser.wonGames);
                                    for (int i = 0; i <= 4; i++)
                                    {
                                        List<ICard> cardList = database.getAllCards();
                                        var random = new Random();
                                        int index = random.Next(cardList.Count);
                                        database.addCardToStack(cardList[index].id, loggedInUser.id);
                                        loggedInUser.stack.addCardToStack(cardList[index]);
                                        Console.WriteLine("You got " + cardList[index].name + "!!!");
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
                        loggedInUser.printProfile();
                        Console.WriteLine();
                        Console.WriteLine("Press 1 to change your password!");
                        Console.WriteLine("Press any key to continue!");
                        tempInput = int.Parse(Console.ReadLine());

                        switch (tempInput)
                        {
                            case 1:
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
                            loggedInUser.stack.printList();
                            tempInput = int.Parse(Console.ReadLine());
                            if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[tempInput-1]))
                            {
                                Console.WriteLine("Whats your minimun damage requirement to trade?");
                                int wantedDamage = int.Parse(Console.ReadLine());
                                Console.WriteLine("Press 1 to search for Spells, press any other key to search for Monsters");
                                int wantedType = int.Parse(Console.ReadLine());
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
                        List<int> cardIDs = database.printActiveTrades(loggedInUser.id);
                        Console.WriteLine("Press 1 to cancel a trade offer");
                        tempInput = int.Parse(Console.ReadLine());
                        if (tempInput == 1)
                        {
                            Console.WriteLine("Select Trade to cancel by Card ID");
                            int cardID = int.Parse(Console.ReadLine());
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
                        List<int> tradeIDs = database.printAllTrades(loggedInUser.id);

                        Console.WriteLine("Select Trade to accept by Card ID");
                        int tradeID = int.Parse(Console.ReadLine());
                        if (tradeIDs.Contains(tradeID))
                        {
                            Console.Clear();
                            loggedInUser.stack.printList();
                            Console.WriteLine("What Card do you want to trade?");
                            tempInput = int.Parse(Console.ReadLine());
                            if (!loggedInUser.deck.cards.Contains(loggedInUser.stack.cards[tempInput-1]))
                            {
                                int[] ids = database.acceptTradeOffer(tradeID, loggedInUser.id);
                                loggedInUser.stack.addCardToStack(database.getCardByID(ids[0]));
                                database.returnCardForTrade(ids[1], tempInput, loggedInUser.id);
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

//Deck Creator verbessern
//Gegnersuche
//Register und Login Nullabfragen
//Token
//Battle Logic alle ausnahmefälle einbinden
//evtl neue Karten erstellen
//Class Diagram
//DB Singleton