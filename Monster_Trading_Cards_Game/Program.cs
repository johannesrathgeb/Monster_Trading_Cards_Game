using Monster_Trading_Cards_Game.Database;
using Monster_Trading_Cards_Game.Cards;
using System;
using System.Collections.Generic;

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
            deck1.addCard(database.getCardByID(1));
            deck1.addCard(database.getCardByID(2));
            deck1.addCard(database.getCardByID(3));
            deck1.addCard(database.getCardByID(4));
            deck1.addCard(database.getCardByID(5));

            CardDeck deck2 = new CardDeck();
            deck2.addCard(database.getCardByID(5));
            deck2.addCard(database.getCardByID(6));
            deck2.addCard(database.getCardByID(7));
            deck2.addCard(database.getCardByID(8));
            deck2.addCard(database.getCardByID(9));

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

            database.Disconnect();

            User newUser1 = new User("TestUser", "PW123", stack, deck1);
            User newUser2 = new User("Userer", "PW123", stack, deck2);

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