using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    interface ICard
    {
        int AdaptDamage(AbstractCard opponent);
    }
}
