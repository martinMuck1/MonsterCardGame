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
        public float Damage { get; private set; }

        public CardModel(string cardID,string name, float damage)
        {
            this.CardID = cardID;
            this.Name = name;
            this.Damage = damage;
        }
    }
}
