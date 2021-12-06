using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class UserDao : IUserDao
    {
        private Database _db;
        public UserDao()
        {
            _db = Database.getInstance();
        }
        public void createUser(UserModel user)
        {
            string sql = "INSERT INTO test(myid,myval) VALUES (1,@myval);";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("myval", "hey");
            //query.Parameters.AddWithValue("Password", user.Password);
            query.Prepare();
            query.ExecuteNonQuery();
            Console.WriteLine("Created new user in DB");
        }
    }
}
