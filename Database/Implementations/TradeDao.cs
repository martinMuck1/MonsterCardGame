using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class TradeDao : Dao, ITradeDao
    {
        public TradeDao()
        {
        }
        public int CreateTradeOffer(TradeModel model)
        {
            try
            {
                string sql = "INSERT INTO trade(\"tid\",\"cardToTrade\",\"cardType\",\"minDamage\", \"fk_uid\") VALUES (@tid,@cardID,@cardType,@minDamage,@uid);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("tid", model.TID);
                query.Parameters.AddWithValue("cardID", model.CardToTrade);
                query.Parameters.AddWithValue("cardType", model.CardType);
                query.Parameters.AddWithValue("minDamage", model.MinDamage);
                query.Parameters.AddWithValue("uid", model.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Error: New Trade offer could not be inserted");
                return -1;
            }
            return 0;
        }

        public int DeleteTrade(TradeModel model)
        {
            try
            {
                string sql = "DELETE FROM trade WHERE \"tid\" = @tid;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("tid", model.TID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Error: Deletion of trade failed");
                return -1;
            }
            return 0;
        }

        public List<TradeModel> ShowAllTradeOffers()
        {
            List<TradeModel> tmpList = new List<TradeModel>();
            string sql = "SELECT * FROM trade ;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            if (!dr.HasRows)
            {
                dr.Close();
                return tmpList;
            }
            while (dr.Read())
            {
                //string type = (dr[2] is DBNull)? "any Type",  
                tmpList.Add(new TradeModel((string)dr[0], (string)dr[1], (string)dr[2], (int)dr[3], (int)dr[4]));
            }
            dr.Close();
            return tmpList;
        }
    }
}
