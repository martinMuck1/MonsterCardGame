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
    }
}
