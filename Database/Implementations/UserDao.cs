using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class UserDao : Dao, IUserDao
    {
        public UserDao()
        {
        }
        public int CreateUser(UserModel user)
        {
            try
            {
                string sql = "INSERT INTO users(username,password) VALUES (@username,@password);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("username", user.Username);
                query.Parameters.AddWithValue("password", user.Password);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: User already exists");
                return -1;
            }
        }

        public int UpdateUserData(UserModel user)
        {
            try
            {
                string sql = "UPDATE users SET name = @name, bio = @bio, image = @image WHERE username = @username;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("name", user.Name);
                query.Parameters.AddWithValue("bio", user.Bio);
                query.Parameters.AddWithValue("image", user.Image);
                query.Parameters.AddWithValue("username", user.Username);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Updating User Data went wrong");
                return -1;
            }
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
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Updating coins failed");
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
                Console.WriteLine(e.Message);
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


        public int GetUserID(string username)
        {
            string sql = "SELECT uid FROM users WHERE username = @username ;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("username", username);
            query.Prepare();
            return (Int32)query.ExecuteScalar();
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> tmpModel = new List<UserModel>();
            string sql = "SELECT uid, username FROM users;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpModel.Add(new UserModel((int)dr[0], (string)dr[1]));
            }
            dr.Close();
            return tmpModel;
        }

        public UserModel GetUserData(string username)
        {
            UserModel tmpModel = null;
            string sql = "SELECT name,bio,image FROM users WHERE username = @username ;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("username", username);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                if (dr[0] is DBNull || dr[1] is DBNull || dr[2] is DBNull)
                {
                    dr.Close();
                    return new UserModel(username,"","","");
                }
                tmpModel = new UserModel(username, (string)dr[0], (string)dr[1], (string)dr[2]);
            }
            dr.Close();
            return tmpModel;
        }

        public int InsertTransaction(TransactionModel model)
        {
            try
            {
                string sql = "INSERT INTO transaction(amount,fk_user,fk_pid) VALUES (@amount,@uid,@pid);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("uid", model.UID);
                query.Parameters.AddWithValue("amount", model.Amount);
                query.Parameters.AddWithValue("pid", model.PackageID);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Transaction failed");
                return -1;
            }
        }

        public List<TransactionModel> GetTransactions(int uid)
        {
            List<TransactionModel> tmpModel = new List<TransactionModel>();
            string sql = "SELECT tid,fk_user,amount,timestamp,fk_pid FROM transaction WHERE fk_user = @uid ORDER BY timestamp DESC;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("uid", uid);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpModel.Add(new TransactionModel((string)dr[0], (int)dr[1], (int)dr[2], (DateTime)dr[3], (string)dr[4]));
            }
            dr.Close();
            return tmpModel;
        }

    }
}
