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
        //TODO consider thread safety => maybe one connection for all threads? 
        //close() needs to be called
        private static Database instance = new Database();
        public NpgsqlConnection Conn { get; set; }

        private Database()
        {
            //TODO get credetials from config file
            this.Conn = new NpgsqlConnection("Server=localhost;User Id=postgres; " +
            "Password=;Database=mctgdb;");
            this.Conn.Open();
        }

        //Get the only object available
        public static Database getInstance()
        {
            return instance;
        }

    }
}
