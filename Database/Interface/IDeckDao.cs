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
        public DeckModel ShowDeckCards(int uid);
    }
}
