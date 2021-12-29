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
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                //Console.WriteLine(e);
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
                    return 0;
                
                return -2;
            }
            catch (Npgsql.PostgresException e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("DB Error: Username or Password wrong");
                return -1;
            }
        }

        public int CheckAccountBalance(string username)
        {
            string sql = "SELECT coins FROM users WHERE username = @username ;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("username", username);
            query.Prepare();
            return (Int32)query.ExecuteScalar();
        }

        public int PayWithCoins(string username, int amount)
        {
            try
            {
                string sql = "UPDATE users SET coins = coins - @amount WHERE username = @username;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("amount", amount);
                query.Parameters.AddWithValue("username", username);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Error: Updating coins failed");
                return -1;
            }
        }

        public int GetUserID(string username)
        {
            string sql = "SELECT uid FROM users WHERE username = @username ;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("username", username);
            query.Prepare();
            return (Int32)query.ExecuteScalar();
        }
        /*
        public int PersistToken(UserModel user)
        {

        }
        */
    }
}
