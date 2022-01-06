using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class Database
    {
        private static Database instance = new Database();
        public NpgsqlConnection Conn { get; set; }

        private Database()
        {
            string[] data = { "", "", "", "" };
            try
            {
                int count = 0;
                foreach (string line in System.IO.File.ReadLines(@"c:\Users\MartinMuck\sem3\MonsterCardGame\Database\credentials.txt"))
                {
                    data[count] = line;
                    count++;
                }
            }catch(Exception e)
            {
                Console.WriteLine("Error with credentials => no db connection");
                Environment.Exit(0);
            }

            this.Conn = new NpgsqlConnection($"Server={data[0]};User Id={data[1]}; " +
            $"Password={data[2]};Database={data[3]};");
            this.Conn.Open();
        }

        //Get the only object available
        public static Database getInstance()
        {
            return instance;
        }

        public void CloseConnection()
        {
            Conn.Close();
        }

    }
}
