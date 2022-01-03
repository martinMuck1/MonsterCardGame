using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public interface ICard
    {
        public string Name { get; }
        public int Damage { get; }  
        public ElementType Element { get; }
        int AdaptDamage(AbstractCard opponent);
    }
}
