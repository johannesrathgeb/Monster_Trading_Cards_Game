using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Menu
{
    public static class UserConfirmation
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!§$%&/()=?*#";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool confirmToken(User loggedInUser)
        {
            DB database = DB.getInstance();
            if (loggedInUser.token != database.getToken(loggedInUser.id))
            {
                Console.Clear();
                Console.WriteLine("You are not logged in anymore!");
                Console.Write("Press any Key to continue...");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        public static User confirmation()
        {
            ConsoleNavigation navigation = new ConsoleNavigation();
            int input = -1;
            DB database = DB.getInstance();
            User loggedInUser = null;
            while (input == -1)
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
            return loggedInUser;
        }
    }
}
