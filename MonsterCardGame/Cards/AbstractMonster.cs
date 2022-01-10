using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public abstract class AbstractMonster : AbstractCard
    {
        protected AbstractMonster(string name, int damage, ElementType element):base(name,damage,element)
        {
        }

        // maybe special spells have no effect monster? 
    }
}
