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
                Console.WriteLine("6.) Exit");
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
                    default:
                        break;
                }
            } while (input != 6);
            database.Disconnect();
        }
    }
}
//TODO
//Deck Creator verbessern
//Gegnersuche
//Trading
//DB Abfragen bearbeiten
//Register und Login Nullabfragen
//Token
//Battle Logic alle ausnahmefälle einbinden
//Unit Testing
//evtl neue Karten erstellen
//Class Diagram