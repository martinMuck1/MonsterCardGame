using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class DeckDao : Dao, IDeckDao
    {
        public DeckDao()
        {
        }
        public int CreateDeck(UserModel user)
        {
            try
            {
                string sql = "INSERT INTO deck(\"fk_uid\") VALUES (@uid);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("uid", user.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: UserDeck already exists");
                return -1;
            }
            return 0;
        }

        public int AddCardsToDeck(DeckModel deck)
        {
            try
            {
                string sql = "UPDATE deck SET card1 = @card1,card2 = @card2,card3 = @card3,card4 = @card4 WHERE \"fk_uid\" = @uid;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("card1", deck.Card[0]);
                query.Parameters.AddWithValue("card2", deck.Card[1]);
                query.Parameters.AddWithValue("card3", deck.Card[2]);
                query.Parameters.AddWithValue("card4", deck.Card[3]);
                query.Parameters.AddWithValue("uid", deck.UID);
                query.Prepare();
                query.ExecuteNonQuery();
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Could not set cards in deck");
                return -1;
            }
            return 0;
        }

        public DeckModel ShowDeckCards(int uid)
        {
            DeckModel tmpModel = null;
            string sql = "SELECT * FROM deck WHERE \"fk_uid\" = @uid;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("uid", uid);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                if(dr[1] is DBNull || dr[2] is DBNull || dr[3] is DBNull || dr[4] is DBNull)
                {
                    dr.Close();
                    return null;
                }
                tmpModel = new DeckModel(uid, (string)dr[0], (string)dr[1], (string)dr[2], (string)dr[3]);
            }
            dr.Close();
            return tmpModel;
        }

    }
}
