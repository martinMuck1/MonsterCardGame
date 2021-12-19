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
        public int CreateUser(UserModel user)
        {
            try
            {
                string sql = "INSERT INTO users(username,password,token) VALUES (@username,@password,@token);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("username", user.Username);
                query.Parameters.AddWithValue("password", user.Password);
                query.Parameters.AddWithValue("token", user.Token);
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

        public int LoginUser(UserModel user)
        {
            try
            {
                string sql = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("username", user.Username);
                query.Parameters.AddWithValue("password", user.Password);
                query.Prepare();
                if((Int64)query.ExecuteScalar() == 1)
                {
                    return 0;
                }
                return -2;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine("DB Error: Query got rejected from DB");
                return -1;
            }
        }
    }
}
