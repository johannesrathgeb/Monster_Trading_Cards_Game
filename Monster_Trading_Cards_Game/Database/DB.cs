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

        public void commandtest()
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@u, @p)", connection))
            {
                cmd.Parameters.AddWithValue("u", "testUser123");
                cmd.Parameters.AddWithValue("p", "testPassword123");
                cmd.ExecuteNonQuery();
            }
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


        public void outputTest()
        {
            using (var cmd = new NpgsqlCommand("SELECT * FROM users", connection))
            {

                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for(int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + "  ");
                    }
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------------------------------------------");
                }
            }
            System.Threading.Thread.Sleep(5000);
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
    }

    
}
