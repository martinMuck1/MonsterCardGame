using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class Kraken: AbstractMonster
    {
        public Kraken(int strenght):base("Kraken", strenght, ElementType.water)
        {
            Console.WriteLine("Release the Kraken!");
        }

    }
}
