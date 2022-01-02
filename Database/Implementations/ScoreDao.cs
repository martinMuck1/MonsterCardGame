using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class ScoreDao : IScoreDao
    {
        private Database _db;
        public ScoreDao()
        {
            _db = Database.getInstance();
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
                tmpModel = new ScoreModel((int)dr[0], (int)dr[1], (int)dr[2], (int)dr[3]);
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
                tmpModel.Add(new ScoreModel((int)dr[0], (int)dr[1], (int)dr[2], (int)dr[3]));
            }
            dr.Close();
            return tmpModel;
        }

    }
}
