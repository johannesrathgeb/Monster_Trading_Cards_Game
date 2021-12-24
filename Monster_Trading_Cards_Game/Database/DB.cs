using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

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
        }

    }
}
