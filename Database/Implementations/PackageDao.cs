using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class PackageDao : IPackageDao
    {
        private Database _db;
        public PackageDao()
        {
            _db = Database.getInstance();
        }

        private void CreateCard(CardModel card, string packageID)
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
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine("DB Error: Inserting Cards into DB was not possible");
            }
        }

        public int CreatePackage(PackageModel package)
        {
            try
            {
                string sql = "INSERT INTO packages(\"packageID\") VALUES (@packageID);";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                query.Parameters.AddWithValue("packageID", package.PackageID);
                query.Prepare();
                query.ExecuteNonQuery();
                
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine("DB Error: PackageID exists already");
                return -1;
            }

            foreach (var card in package.PackageCards)  //falls Card ID doppelt => nicht erfolgreich = sehr unwahrscheinlich
            {
                CreateCard(card, package.PackageID);
            }

            return 0;
        }
    }
}
