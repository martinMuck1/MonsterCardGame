using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;

namespace MonsterCardGame.Battle
{
    public class Deck : AbstractDeckManager
    {
        const int _deckSize = 4;
        public Deck(List<AbstractCard> cardList)
        {
            this._cardCollection = cardList;
        }
    }
}
