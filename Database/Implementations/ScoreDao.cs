using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class ScoreDao : Dao, IScoreDao
    {
        public ScoreDao()
        {
        }

        public int CreateScoreStats(UserModel user)
        {
            try
            {
                string sql = "INSERT INTO scoreboard(\"fk_uid\") VALUES (@uid);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("uid", user.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Error: Userstats already exists");
                return -1;
            }
            return 0;
        }

        public ScoreModel ShowUserStats(int uid)
        {
            ScoreModel tmpModel = null;
            string sql = "SELECT * FROM scoreboard WHERE \"fk_uid\" = @uid;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("uid", uid);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpModel = new ScoreModel((int)dr[0], (int)dr[1], (int)dr[2], (int)dr[3], (int)dr[4]);
            }
            dr.Close();
            return tmpModel;
        }

        public List<ScoreModel> ShowScoreBoard()
        {
            List<ScoreModel> tmpModel = new List<ScoreModel>();
            string sql = "SELECT * FROM scoreboard ORDER BY elo DESC;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpModel.Add(new ScoreModel((int)dr[0], (int)dr[1], (int)dr[2], (int)dr[3], (int)dr[4]));
            }
            dr.Close();
            return tmpModel;
        }

        //shows oldest battle request
        public BattleModel ShowRequests()
        {
            //Monitor.Enter(_db);
            BattleModel tmpModel = null;
            string sql = "SELECT bid,\"fk_uid\" FROM battle WHERE request = true ORDER BY timestamp ASC LIMIT 1;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                return null;
            }
            while (dr.Read())
            {
                if (dr[0] is DBNull || dr[1] is DBNull)
                {
                    dr.Close();
                    return null;
                }
                tmpModel = new BattleModel((string)dr[0], (int)dr[1]);
            }
            dr.Close();
            //Monitor.Exit(_db);
            return tmpModel;
        }
        public int CreateBattleRequest(BattleModel model)
        {
            //Monitor.Enter(_db);
            try
            {
                string sql = "INSERT INTO battle(\"fk_uid\") VALUES (@uid);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("uid", model.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: User already set request");
                return -1;
            }
            //Monitor.Exit(_db);
            return 0;
        }
        public int UpdateScoreBoard(ScoreModel model)
        {
            //Monitor.Enter(_db);
            try
            {
                string sql = "UPDATE scoreboard SET elo = elo + @elo,wins = wins + @wins,games = games+ @games,loses = loses+ @loses WHERE \"fk_uid\" = @uid;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("elo", model.Elo);
                query.Parameters.AddWithValue("wins", model.Wins);
                query.Parameters.AddWithValue("games", model.Games);
                query.Parameters.AddWithValue("loses", model.Loses);
                query.Parameters.AddWithValue("uid", model.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Could not update user stats");
                return -1;
            }
            //Monitor.Exit(_db);
            return 0;
        }
        
        public int DeleteBattleRequest(BattleModel model)
        {
            //Monitor.Enter(_db);
            try
            {
                string sql = "DELETE FROM battle WHERE \"bid\" = @bid;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("bid", model.BID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Could not delete request");
                return -1;
            }
            //Monitor.Exit(_db);
            return 0;
        }
    }
}
