using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class CardDao : ICardDao
    {
        private Database _db;
        public CardDao()
        {
            _db = Database.getInstance();
        }

        public int CreateCard(CardModel card, string packageID)
        {
            try
            {
                string sql = "INSERT INTO cards(\"cardID\",\"name\",\"damage\",\"fk_packageID\") VALUES (@cardID,@name,@damage,@packageID);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("cardID", card.CardID);
                query.Parameters.AddWithValue("name", card.Name);
                query.Parameters.AddWithValue("damage", card.Damage);
                query.Parameters.AddWithValue("packageID", packageID);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("DB Error: Inserting Cards into DB was not possible");
                return -1;
            }
        }

        public int ChangeCardsOwner(CardModel card)
        {
            try
            {
                string sql = "UPDATE cards SET owner = @username WHERE \"cardID\" = @cardID;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("username", card.UID);
                query.Parameters.AddWithValue("cardID", card.CardID);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                //Console.WriteLine(e);
                Console.WriteLine("DB Error: Updating card failed");
                return -1;
            }
        }

        public List<CardModel> showPackageCards(PackageModel package)
        {
            List<CardModel> tmpList = new List<CardModel>();
            string sql = "SELECT \"cardID\",name,damage FROM cards WHERE \"fk_packageID\" = @package;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("package", package.PackageID);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpList.Add(new CardModel((string)dr[0], (string)dr[1], (double)dr[2]));
            }
            dr.Close();
            return tmpList;
        }

        public List<CardModel> ShowAquiredCards(int username)
        {
            List<CardModel> tmpList = new List<CardModel>();
            string sql = "SELECT \"cardID\",name,damage FROM cards WHERE \"owner\" = @username;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("username", username);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpList.Add(new CardModel((string)dr[0], (string)dr[1], (double)dr[2]));
            }
            dr.Close();
            return tmpList;
        }

        public CardModel ShowSingleCard(string cardID)
        {
            CardModel tmpCard = null;
            string sql = "SELECT \"cardID\",name,damage FROM cards WHERE \"cardID\" = @cardID;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Parameters.AddWithValue("cardID", cardID);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                tmpCard = new CardModel((string)dr[0], (string)dr[1], (double)dr[2]);
            }
            dr.Close();
            return tmpCard;
        }
    }
}
