using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class TradeModel

    {
        public string TID { get; private set; }
        public string CardToTrade { get; private set; }
        public string CardType { get; private set; }
        public int  MinDamage { get; private set; }
        public int UID { get; set; }
        public string Username { get; set; }

        public TradeModel(string tid, string cardID, string cardtype, int mindamage, int uid)
        {
            this.TID = tid;
            this.CardToTrade = cardID;
            this.CardType = cardtype;
            this.MinDamage = mindamage;
            this.UID = uid;
        }

        public TradeModel(string tid, string cardID, string cardtype, int mindamage, string username)
        {
            this.TID = tid;
            this.CardToTrade = cardID;
            this.CardType = cardtype;
            this.MinDamage = mindamage;
            this.Username = username;
            SetUID();
        }
        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }
    }
}
