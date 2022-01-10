using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Newtonsoft.Json;

namespace Monster_Trading_Cards_Game.Database
{
    public class DB
    {
        const string ConnString = "Host=localhost;Username=postgres;Password=;Database=postgres";
        private NpgsqlConnection connection;
        private NpgsqlCommand cmd;
        private NpgsqlDataReader reader;
        //private NpgsqlBatchCommand testCommand;

        public NpgsqlConnection Connect()
        {
            connection = new NpgsqlConnection(ConnString);
            connection.Open();
            return connection;
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public void createCard(string name, double damage, ICard.Element_type elementType, ICard.Monster_type monsterType, ICard.Monster_type weakness)
        {
            cmd = new NpgsqlCommand("INSERT INTO cards (name, damage, elementType, monsterType, weakness) VALUES (@n, @d, @e, @m, @w)", connection);
            cmd.Parameters.AddWithValue("n", name);
            cmd.Parameters.AddWithValue("d", damage);
            cmd.Parameters.AddWithValue("e", (int) elementType);
            cmd.Parameters.AddWithValue("m", (int) monsterType);
            cmd.Parameters.AddWithValue("w", (int) weakness);
            cmd.ExecuteNonQuery();
            
        }

        public void addCardToStack(int cardID, int userID)
        {
            cmd = new NpgsqlCommand("INSERT INTO stacks (userID, cardID) VALUES (@u, @c)", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
            
        }

        public void createUser(string username, string password)
        {
            cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@u, @p)", connection);
            cmd.Parameters.AddWithValue("u", username);
            cmd.Parameters.AddWithValue("p", password);
            cmd.ExecuteNonQuery();
        }

        public void addCardToDeck(int cardID, int userID)
        {
            cmd = new NpgsqlCommand("INSERT INTO decks (userID, cardID) VALUES (@u, @c)", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
            
        }

        public void tradeCard(int cardID, int userID, int wantedDamage, int wantedType)
        {
            cmd = new NpgsqlCommand("DELETE FROM stacks WHERE id IN(SELECT id FROM stacks WHERE userID = @u AND cardID = @c LIMIT 1);", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand("INSERT INTO trades (ownerID, cardID, wantedDamage, wantedType) VALUES (@u, @c, @d, @t)", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.Parameters.AddWithValue("d", wantedDamage);
            cmd.Parameters.AddWithValue("t", wantedType);
            cmd.ExecuteNonQuery();
        }

        public List<int> activeTradesCardIDs(int userID)
        {
            List<int> cardIDs = new List<int>();
            cmd = new NpgsqlCommand("SELECT cardID, wantedDamage, wantedType, name, damage FROM trades JOIN cards ON trades.cardID = cards.cid WHERE ownerID = @o;", connection);
            cmd.Parameters.AddWithValue("o", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    cardIDs.Add((int)reader[0]);
                }
            }
            reader.Close();
            return cardIDs;
        }

        public string activeTradesPrint(int userID)
        {
            string print = null;
            cmd = new NpgsqlCommand("SELECT cardID, wantedDamage, wantedType, name, damage FROM trades JOIN cards ON trades.cardID = cards.cid WHERE ownerID = @o;", connection);
            cmd.Parameters.AddWithValue("o", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read()){
                if (reader.HasRows)
                {
                    print += (reader[3] + " with Damage: " + reader[4] + " wanted Damage: " + reader[1] + " wanted Type: " + reader[2] + '\n');
                }
            }
            reader.Close();
            return print;
        }

        public string activeTradesChoose(int userID)
        {
            string print = null;
            cmd = new NpgsqlCommand("SELECT cardID, wantedDamage, wantedType, name, damage FROM trades JOIN cards ON trades.cardID = cards.cid WHERE ownerID = @o;", connection);
            cmd.Parameters.AddWithValue("o", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    print += ("[ ]" + reader[3] + " with Damage: " + reader[4] + " wanted Damage: " + reader[1] + " wanted Type: " + reader[2] + '\n');
                }
            }
            reader.Close();
            return print;
        }

        public string printAllTrades(int userID)
        {
            string print = null;
            cmd = new NpgsqlCommand("SELECT id, wantedDamage, wantedType, name, damage, username FROM ((trades JOIN cards ON trades.cardID = cards.cid) JOIN users ON trades.ownerID = users.userid) WHERE ownerID != @o;", connection);
            cmd.Parameters.AddWithValue("o", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    if ((int)reader[2] == 1)
                    {
                        print += ("[ ] " + reader[5] + " trades " + reader[3] + " with " + reader[4] + " damage for Spell with minimum " + reader[1] + " damage!" + '\n');
                    }
                    else
                    {
                        print += ("[ ] " + reader[5] + " trades " + reader[3] + " with " + reader[4] + " damage for Monster with minimum " + reader[1] + " damage!" + '\n');
                    }
                }
            }
            reader.Close();
            return print;
        }

        public List<int> allTradeIDs(int userID)
        {
            List<int> tradeIDs = new List<int>();
            cmd = new NpgsqlCommand("SELECT id, wantedDamage, wantedType, name, damage, username FROM ((trades JOIN cards ON trades.cardID = cards.cid) JOIN users ON trades.ownerID = users.userid) WHERE ownerID != @o;", connection);
            cmd.Parameters.AddWithValue("o", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    tradeIDs.Add((int)reader[0]);
                }
            }
            reader.Close();
            return tradeIDs;
        }

        public void cancelTrade(int cardID, int userID)
        {
            cmd = new NpgsqlCommand("DELETE FROM trades WHERE id IN(SELECT id FROM trades WHERE ownerID = @o AND cardID = @c LIMIT 1);", connection);
            cmd.Parameters.AddWithValue("o", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand("INSERT INTO stacks (userID, cardID) VALUES (@u, @c)", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
        }

        public int[] acceptTradeOffer(int tradeID, int userID)
        {
            int[] ids = new int[2];
            cmd = new NpgsqlCommand("SELECT cardID, ownerID FROM trades WHERE id = @i", connection);
            cmd.Parameters.AddWithValue("i", tradeID);
            reader = cmd.ExecuteReader();
            reader.Read();
            ids[0] = (int)reader[0];
            ids[1] = (int)reader[1];
            reader.Close();

            cmd = new NpgsqlCommand("INSERT INTO stacks (userID, cardID) VALUES (@u, @c) ", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", ids[0]);
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand("DELETE FROM trades WHERE id = @i;", connection);
            cmd.Parameters.AddWithValue("i", tradeID);
            cmd.ExecuteNonQuery();
            return ids;
        }

        public void returnCardForTrade(int ownerID, int cardID, int userID)
        {
            cmd = new NpgsqlCommand("INSERT INTO stacks (userID, cardID) VALUES (@u, @c) ", connection);
            cmd.Parameters.AddWithValue("u", ownerID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();

            cmd = new NpgsqlCommand("DELETE FROM stacks WHERE id IN(SELECT id FROM stacks WHERE userID = @u AND cardID = @c LIMIT 1);", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
        }

        public void removeCardFromDeck(int cardID, int userID)
        {
            cmd = new NpgsqlCommand("DELETE FROM decks WHERE id IN(SELECT id FROM decks WHERE userID = @u AND cardID = @c LIMIT 1);", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
        }

        public string getUserPW(string username)
        {
            cmd = new NpgsqlCommand("SELECT password FROM users WHERE username = @u", connection);
            cmd.Parameters.AddWithValue("u", username);
            reader = cmd.ExecuteReader();
            reader.Read();
            string password = (string)reader[0];
            reader.Close();
            return password;
            
        }

        public bool userExists(string username)
        {
            bool exists = false;
            cmd = new NpgsqlCommand("SELECT FROM users WHERE username = @u;", connection);
            cmd.Parameters.AddWithValue("u", username);
            reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                exists = true;
            }
            reader.Close();
            return exists;            
        }

        public User loginUser(string username)
        {
            cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @u", connection);
            cmd.Parameters.AddWithValue("u", username);
            reader = cmd.ExecuteReader();
            reader.Read();
            int userID = (int)reader[0];
            reader.Close();
            CardStack stack = getStackByID(userID);
            CardDeck deck = getDeckByID(userID);
            cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = @u", connection);
            cmd.Parameters.AddWithValue("u", username);
            NpgsqlDataReader reader1 = cmd.ExecuteReader();
            reader1.Read();
            User currentUser = new User(userID, username, (string)reader1[2], (int)reader1[3], (int)reader1[4], (int)reader1[5], (int)reader1[6], stack, deck);
            reader1.Close();
            return currentUser;
        }

        public void updatePassword(int id, string password)
        {
            cmd = new NpgsqlCommand("UPDATE users SET password = @p WHERE userid = @i;", connection);
            cmd.Parameters.AddWithValue("i", id);
            cmd.Parameters.AddWithValue("p", password);
            cmd.ExecuteNonQuery();
        }

        public void printScoreboard()
        {
            Console.WriteLine("Name     Elo");
            cmd = new NpgsqlCommand("SELECT username, elo FROM users ORDER BY elo DESC;", connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0] + "     " + reader[1]);
            }
        }

        public CardStack getStackByID(int userID)
        {
            List<int> cardIDs = new List<int>();
            List<ICard> cardList = new List<ICard>();
            cmd = new NpgsqlCommand("SELECT cardID FROM stacks WHERE userID = @u", connection);
            cmd.Parameters.AddWithValue("u", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    cardIDs.Add((int)reader[i]);   
                }
            }
            reader.Close();
            foreach(int id in cardIDs)
            {
                cardList.Add(getCardByID(id));
            }
            CardStack stack = new CardStack(cardList);
            return stack;
        }

        public int[] tradeWanteds(int tradeID)
        {
            cmd = new NpgsqlCommand("SELECT wantedDamage, wantedType FROM trades WHERE id = @i", connection);
            cmd.Parameters.AddWithValue("i", tradeID);
            reader = cmd.ExecuteReader();
            reader.Read();
            int[] wanteds = new int[2];
            wanteds[0] = (int)reader[0];
            wanteds[1] = (int)reader[1];
            reader.Close();
            return wanteds;
        }

        public CardDeck getDeckByID(int userID)
        {
            List<int> cardIDs = new List<int>();
            List<ICard> cardList = new List<ICard>();
            cmd = new NpgsqlCommand("SELECT cardID FROM decks WHERE userID = @u", connection);
            cmd.Parameters.AddWithValue("u", userID);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    cardIDs.Add((int)reader[i]);
                }
            }
            reader.Close();
            foreach (int id in cardIDs)
            {
                cardList.Add(getCardByID(id));
            }
            CardDeck deck = new CardDeck(cardList);
            return deck;
        }

        public ICard getCardByID(int id)
        {
            cmd = new NpgsqlCommand("SELECT * FROM cards WHERE cid = @i", connection);
            cmd.Parameters.AddWithValue("i", id);
            reader = cmd.ExecuteReader();
            reader.Read();
            if((int)reader[4] == 7)
            {
                SpellCard card = new SpellCard((int)reader[0], (string)reader[1], (int)reader[2], (ICard.Element_type)reader[3], (ICard.Monster_type)reader[5]);
                reader.Close();
                return card;
            }
            else
            {
                MonsterCard card = new MonsterCard((int)reader[0], (string)reader[1], (int)reader[2], (ICard.Element_type)reader[3], (ICard.Monster_type)reader[4],(ICard.Monster_type)reader[5]);
                reader.Close();
                return card;
            }          
        }

        public List<ICard> getAllCards()
        {
            List<ICard> cardList = new List<ICard>();
            cmd = new NpgsqlCommand("SELECT * FROM cards", connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if ((int)reader[4] == 7)
                {
                    SpellCard card = new SpellCard((int)reader[0], (string)reader[1], (int)reader[2], (ICard.Element_type)reader[3], (ICard.Monster_type)reader[5]);
                    cardList.Add(card);
                }
                else
                {
                    MonsterCard card = new MonsterCard((int)reader[0], (string)reader[1], (int)reader[2], (ICard.Element_type)reader[3], (ICard.Monster_type)reader[4], (ICard.Monster_type)reader[5]);
                    cardList.Add(card);
                }
            }
            reader.Close();
            return cardList;
        }

        public void updateUserStats(int id, int coins, int elo, int playedGames, int wonGames)
        {
            cmd = new NpgsqlCommand("UPDATE users SET coins = @c, elo = @e, games_played = @p, games_won = @w WHERE userid = @i", connection);
            cmd.Parameters.AddWithValue("c", coins);
            cmd.Parameters.AddWithValue("e", elo);
            cmd.Parameters.AddWithValue("p", playedGames);
            cmd.Parameters.AddWithValue("w", wonGames);
            cmd.Parameters.AddWithValue("i", id);
            cmd.ExecuteNonQuery(); 
        }
    }
}
