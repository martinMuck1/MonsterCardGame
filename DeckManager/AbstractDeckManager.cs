using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;

namespace MonsterCardGame.DeckManage
{
    class AbstractDeckManager
    {
        protected List<AbstractCard> stackCards = new List<AbstractCard>();
        public AbstractDeckManager()
        {

        }
        public void AddCardToStack(AbstractCard card)
        {

        }

        public void RemoveCardFromStack()
        {

        }

        public AbstractCard FindCardInStack(AbstractCard card)
        {
            return new WaterGoblin(100);
        }
    }
}
