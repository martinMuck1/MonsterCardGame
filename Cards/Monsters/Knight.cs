using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class Knight: AbstractMonster
    {
        public Knight(int strenght):base("Knight", strenght, ElementType.normal)
        {
            Console.WriteLine("Knight entered the room");
        }
        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {
            if (opponent is WaterSpell)
            {
                Console.WriteLine($"{this.Name} drowned due to heavy armor in water !");
                return 0;
            }
            return tmpDamage;
        }

    }
}
