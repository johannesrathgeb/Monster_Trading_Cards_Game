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
    class DB
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
            using (var cmd = new NpgsqlCommand("INSERT INTO cards (name, damage, elementType, monsterType, weakness) VALUES (@n, @d, @e, @m, @w)", connection))
            {
                cmd.Parameters.AddWithValue("n", name);
                cmd.Parameters.AddWithValue("d", damage);
                cmd.Parameters.AddWithValue("e", (int) elementType);
                cmd.Parameters.AddWithValue("m", (int) monsterType);
                cmd.Parameters.AddWithValue("w", (int) weakness);
                cmd.ExecuteNonQuery();
            }
        }

        public void addCardToStack(int cardID, int userID)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO stacks (userID, cardID) VALUES (@u, @c)", connection))
            {
                cmd.Parameters.AddWithValue("u", userID);
                cmd.Parameters.AddWithValue("c", cardID);
                cmd.ExecuteNonQuery();
            }
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
            using (var cmd = new NpgsqlCommand("INSERT INTO decks (userID, cardID) VALUES (@u, @c)", connection))
            {
                cmd.Parameters.AddWithValue("u", userID);
                cmd.Parameters.AddWithValue("c", cardID);
                cmd.ExecuteNonQuery();
            }
        }
//----------------------------------------------------------------------------------------------------------------------------
        public void removeCardFromDeck(int cardID, int userID)
        {
            cmd = new NpgsqlCommand("DELETE FROM decks WHERE userID = @u AND cardID = @c;", connection);
            cmd.Parameters.AddWithValue("u", userID);
            cmd.Parameters.AddWithValue("c", cardID);
            cmd.ExecuteNonQuery();
        }
//---------------------------------------------------------------------------------------------------------------------------
        public string getUserPW(string username)
        {
            using (var cmd = new NpgsqlCommand("SELECT password FROM users WHERE username = '" + username + "'", connection))
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                string password = (string)reader[0];
                reader.Close();
                return password;
            }
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
            using (var cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = '" + username + "'", connection))
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int userID = (int)reader[0];
                reader.Close();
                CardStack stack = getStackByID(userID);
                CardDeck deck = getDeckByID(userID);
                NpgsqlDataReader reader1 = cmd.ExecuteReader();
                reader1.Read();
                User currentUser = new User(userID, username, (string)reader1[2], (int)reader1[3], (int)reader1[4], (int)reader1[5], (int)reader1[6], stack, deck);
                reader1.Close();
                return currentUser;
            }
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
            using (var cmd = new NpgsqlCommand("SELECT cardID FROM stacks WHERE userID = " + userID, connection))
            {
                List<int> cardIDs = new List<int>();
                List<ICard> cardList = new List<ICard>();
                NpgsqlDataReader reader = cmd.ExecuteReader();
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
        }

        public CardDeck getDeckByID(int userID)
        {
            using (var cmd = new NpgsqlCommand("SELECT cardID FROM decks WHERE userID = " + userID, connection))
            {
                List<int> cardIDs = new List<int>();
                List<ICard> cardList = new List<ICard>();
                NpgsqlDataReader reader = cmd.ExecuteReader();
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
        }

        public ICard getCardByID(int id)
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM cards WHERE cid = " + id , connection))
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
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
        }

        public List<ICard> getAllCards()
        {
            List<ICard> cardList = new List<ICard>();
            using (var cmd = new NpgsqlCommand("SELECT * FROM cards", connection))
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
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
            }
            return cardList;
        }

        public void updateUserStats(int id, int coins, int elo, int playedGames, int wonGames)
        {
            using (var cmd = new NpgsqlCommand("UPDATE users SET coins = " + coins + ", elo = " + elo + ", games_played = " + playedGames + ", games_won = " + wonGames + " WHERE userid = " + id, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
