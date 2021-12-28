using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface ICardDao
    {
        public int CreateCard(CardModel card, string packageID);
    }
}
