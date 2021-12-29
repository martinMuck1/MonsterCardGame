using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface IDeckDao
    {
        public int CreateDeck(UserModel user);
        public int AddCardsToDeck(DeckModel deck);
        public DeckModel ShowDeckCards(int uid);
    }
}
