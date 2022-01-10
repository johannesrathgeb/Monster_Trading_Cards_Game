using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monster_Trading_Cards_Game.Database;

namespace Monster_Trading_Cards_Game.Menu
{
    static class startBattle
    {
        public static void start(User loggedInUser)
        {
            DB database = DB.getInstance();
            if (loggedInUser.deck.cardCount() >= 4)
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
        }
    }
}
