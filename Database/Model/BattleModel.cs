using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class BattleModel

    {
        public string Username { get; private set; }
        public int UID { get; set; }
        public string BID { get; private set; }

        public BattleModel(string username)
        {
            this.Username = username;
            SetUID();
        }
        public BattleModel(string bid, int uid)
        {
            this.BID = bid;
            this.UID = uid;
        }

        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }


    }
}
