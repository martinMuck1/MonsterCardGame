using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class TransactionModel
    {
        public string Username { get; private set; }
        public int UID { get; set; }
        public string TID { get; private set; }
        public int Amount { get; private set; }
        public string Timestamp { get; private set; }
        public string PackageID { get; private set; }


        public TransactionModel(string username, int amount, string packageID)
        {
            this.Username = username;
            this.Amount = amount;
            this.PackageID = packageID;
            SetUID();
            //this.Token = token;
        }

        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }


    }
}
