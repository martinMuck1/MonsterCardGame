using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class NormalSpell : AbstractSpell  
    {
        public NormalSpell(int strenght) : base("NormalSpell", strenght, ElementType.normal)
        {
            //Console.WriteLine("Here comes WaterGoblin");
        }

    }
}
