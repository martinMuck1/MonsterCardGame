using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class NormalWizzard: AbstractMonster
    {
        public NormalWizzard(int strenght):base("Wizzard", strenght, ElementType.normal)
        {
            //Console.WriteLine("Knight entered the room");
        }

    }
}
