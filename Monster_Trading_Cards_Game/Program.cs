using Monster_Trading_Cards_Game.Database;
using Monster_Trading_Cards_Game.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

namespace Monster_Trading_Cards_Game
{
    class Program
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


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
                            string token = (RandomString(16));
                            database.setToken(token, loggedInUser.id);
                            loggedInUser.token = token;
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
                    Console.WriteLine("[ ] Create own Card");
                    Console.WriteLine("[ ] Profile");
                    Console.WriteLine("[ ] Scoreboard");
                    Console.WriteLine("[ ] Trade Card");
                    Console.WriteLine("[ ] Edit active Trades");
                    Console.WriteLine("[ ] Watch Trade Offers");
                    Console.WriteLine("[ ] Exit");
                    input = navigation.moveCursor(10, Console.CursorTop);
                }
                if(loggedInUser.token != database.getToken(loggedInUser.id))
                {
                    Console.Clear();
                    Console.WriteLine("You are not logged in anymore!");
                    Console.Write("Press any Key to continue...");
                    Console.ReadLine();
                    return;
                }
                switch (input)
                {
                    case 1:
                        loggedInUser.setDeck();
                        break;
                    case 2:
                        if(loggedInUser.deck.cardCount() >= 4)
                        {
                            User enemyUser = database.getEnemyUser(loggedInUser.id);
                            Battle battle = new Battle(loggedInUser, enemyUser);
                            battle.fight();
                            database.updateUserStats(loggedInUser.id, loggedInUser.coins, loggedInUser.elo, loggedInUser.playedGames, loggedInUser.wonGames);
                            database.updateUserStats(enemyUser.id, enemyUser.coins, enemyUser.elo, enemyUser.playedGames, enemyUser.wonGames);
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
                            tempInput = -1;
                            while (tempInput == -1)
                            {
                                Console.Clear();
                                Console.WriteLine("Do you want to create a Card for 10 coins?");
                                Console.WriteLine("[ ] Yes");
                                Console.WriteLine("[ ] No");
                                tempInput = navigation.moveCursor(2, Console.CursorTop);
                            }

                            switch (tempInput)
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
                                        carddamage= int.Parse(Console.ReadLine());
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
                                    if(monstertype == ICard.Monster_type.Spell)
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
                        break;
                    case 5:
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
                    case 6:
                        database.printScoreboard();
                        Console.Write("Press any Key to continue...");
                        Console.ReadLine();
                        break;
                    case 7:
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
                    case 8:
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
                    case 9:
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
            } while (input != 10);
            database.Disconnect();
        }
    }
}
//TODO
//Class Diagram