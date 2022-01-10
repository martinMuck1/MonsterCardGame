using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class CardModel
    {
        public string CardID { get; private set; }
        public string Name { get; private set; }
        public double Damage { get; private set; }
        public string Username { get;  set; }
        public int UID { get; private set; }

        public CardModel(string cardID,string name, double damage)
        {
            this.CardID = cardID;
            this.Name = name.Trim();
            this.Damage = damage;
        }
        public CardModel(string cardID, string username)
        {
            this.CardID = cardID;
            this.Username = username;
        }
        public CardModel(string cardID, int uid)
        {
            this.CardID = cardID;
            this.UID = uid;
        }
        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }
    }
}
