using Monster_Trading_Cards_Game.Database;
using Monster_Trading_Cards_Game.Cards;
using Monster_Trading_Cards_Game.Menu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

namespace Monster_Trading_Cards_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            //Upload Cards to Database
            //UploadCardsToDB cardUpload = new UploadCardsToDB();
            //cardUpload.upload();

            //DB Connection
            DB database = DB.getInstance();
            database.Connect();

            ConsoleNavigation navigation = new ConsoleNavigation();
            User loggedInUser = UserConfirmation.confirmation();
            int input;

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
                Console.Clear();
                if (!UserConfirmation.confirmToken(loggedInUser))
                {
                    return;
                }
                switch (input)
                {
                    case 1:
                        loggedInUser.setDeck();
                        break;
                    case 2:
                        startBattle.start(loggedInUser);
                        break;
                    case 3:
                        Shop.packs(loggedInUser);
                        break;
                    case 4:
                        Shop.ownCard(loggedInUser);
                        break;
                    case 5:
                        Stats.profile(loggedInUser);
                        break;
                    case 6:
                        Stats.scores();
                        break;
                    case 7:
                        Trading.tradeCard(loggedInUser);
                        break;
                    case 8:
                        Trading.editTrades(loggedInUser);
                        break;
                    case 9:
                        Trading.watchTrades(loggedInUser);
                        break;
                    default:
                        break;
                }
            } while (input != 10);
            database.Disconnect();
        }
    }
}