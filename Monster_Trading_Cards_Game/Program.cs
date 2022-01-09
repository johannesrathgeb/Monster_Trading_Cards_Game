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

            

            User newUser1 = new User(0, "TestUser", "PW123", 20, 100, stack, deck1);
            User newUser2 = new User(0, "Userer", "PW123", 20, 100, stack, deck2);

            //Actual Program
            int input;
            User loggedInUser = null;
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

            do
            {
                Console.Clear();
                Console.WriteLine("1.) Edit Deck");
                Console.WriteLine("2.) Fight");
                Console.WriteLine("3.) Add Card to Stack");
                Console.WriteLine("5.) Exit");
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
                        }
                        else
                        {
                            Console.WriteLine("Your deck contains less than 4 cards!");
                            Console.Write("Press any Key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    case 3:
                        //List all Cards
                        List<ICard> cardList = database.getAllCards();
                        foreach(ICard card in cardList)
                        {
                            Console.WriteLine(card.id + " " + card.name);
                        }
                        Console.WriteLine("Enter ID of Card you want to add to your Stack");
                        
                        //Choose
                        int cardID = int.Parse(Console.ReadLine());
                        ICard chosenCard = database.getCardByID(cardID);
                        if (!loggedInUser.stack.isCardInStack(chosenCard.id))
                        {
                            //Add to Stack
                            database.addCardToStack(chosenCard.id, loggedInUser.id);
                            loggedInUser.stack.addCardToStack(chosenCard);
                        }
                        break;
                    default:
                        break;
                }
            } while (input != 5);
            database.Disconnect();
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