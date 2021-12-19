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
        public int createUser(UserModel user)
        {
            try
            {
                string sql = "INSERT INTO users(username,password) VALUES (@username,@password);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("username", user.Username);
                query.Parameters.AddWithValue("password", user.Password);
                query.Prepare();
                query.ExecuteNonQuery();
                Console.WriteLine("Created new user in DB");
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine("DB Error: User already exists");
                return -1;
            }
        }
    }
}
