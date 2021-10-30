using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.DeckManage
{
    class Deck : AbstractDeckManager
    {
        public Deck()
        {
            for (int counter = 0; counter < 4; counter++)   //add 4 cards to deck
            {
                this.cardCollection.Add(CreateRandomCard());
            }
        }
    }
}
