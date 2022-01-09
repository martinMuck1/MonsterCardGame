using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class PackageDao : Dao, IPackageDao
    {
        public PackageDao()
        {
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
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: PackageID exists already");
                return -1;
            }
            return 0;
        }

        public List<PackageModel> GetAllUnaquiredPackages()
        {
            List<PackageModel> packageList = new List<PackageModel>();
            string sql = "SELECT \"packageID\" FROM packages WHERE aquired IS false;";
            using var query = new NpgsqlCommand(sql, _db.Conn);
            query.Prepare();
            NpgsqlDataReader dr = query.ExecuteReader();
            while (dr.Read())
            {
                packageList.Add(new PackageModel(dr[0].ToString()));
                //Console.WriteLine(dr[0]);
            }
            dr.Close();
            return packageList;
        }

        public int AquirePackage(PackageModel aquiredPackage)
        {
            try
            {
                //string sql = "UPDATE packages SET owner = @username WHERE \"packageID\" = @packageID;";
                string sql = "UPDATE packages SET aquired = true WHERE \"packageID\" = @packageID;";
                using var query = new NpgsqlCommand(sql, _db.Conn);
                //query.Parameters.AddWithValue("username", aquiredPackage.UID);
                query.Parameters.AddWithValue("packageID", aquiredPackage.PackageID);
                query.Prepare();
                query.ExecuteNonQuery();
                return 0;
            }
            catch (Npgsql.PostgresException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("DB Error: Updating package failed");
                return -1;
            }
        }
    }
}
