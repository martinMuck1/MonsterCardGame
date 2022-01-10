using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class FireTroll : AbstractMonster
    {
        public FireTroll(int strenght) : base("Troll", strenght, ElementType.fire)
        {
            //Console.WriteLine("Here comes Dragon");
        }

    }
}
