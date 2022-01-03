using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class FireSpell : AbstractSpell  
    {
        public FireSpell(int strenght) : base("FireSpell", strenght, ElementType.fire)
        {
            //Console.WriteLine("Here comes WaterGoblin");
        }

    }
}
