using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class WaterSpell : AbstractSpell  
    {
        public WaterSpell(int strenght) : base("WaterSpell", strenght, ElementType.water)
        {
            //Console.WriteLine("Here comes WaterGoblin");
        }

    }
}
