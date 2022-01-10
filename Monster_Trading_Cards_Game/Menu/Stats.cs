using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Menu
{
    static class Stats
    {
        public static void profile(User loggedInUser)
        {
            int input;
            ConsoleNavigation navigation = new ConsoleNavigation();
            DB database = DB.getInstance();
            input = -1;
            while (input == -1)
            {
                Console.Clear();
                loggedInUser.printProfile();
                Console.WriteLine("[ ] Return to menu");
                Console.WriteLine("[ ] Change password");
                input = navigation.moveCursor(2, Console.CursorTop);
            }
            Console.Clear();
            switch (input)
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
        }

        public static void scores()
        {
            DB database = DB.getInstance();
            database.printScoreboard();
            Console.Write("Press any Key to continue...");
            Console.ReadLine();
        }
    }
}
