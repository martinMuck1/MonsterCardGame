using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class FireElves : AbstractMonster
    {
        public FireElves(int strenght) : base("Elves", strenght, ElementType.fire)
        {
            //Console.WriteLine("Here comes Dragon");
        }

    }
}
