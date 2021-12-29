using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class DeckModel
    {
        public string Username { get; private set; }
        public int UID { get; private set; }
        public string[] Card { get; private set; }

        public DeckModel(int uid,string card1, string card2,string card3, string card4)
        {
            this.UID = uid;
            this.Card[0] = card1;
            this.Card[1] = card2;
            this.Card[2] = card3;
            this.Card[3] = card4;
        }
    }
}
