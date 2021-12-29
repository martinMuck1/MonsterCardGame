using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class PackageModel
    {
        public string PackageID { get; private set; }

        public PackageModel(string packageID)
        {
            this.PackageID = packageID;
        }

        /*
        public PackageModel(string packageID, string username)
        {
            this.PackageID = packageID;
            this.Username = username;
        }

        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }
        */
    }
}
