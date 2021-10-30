using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class FireDragon : AbstractMonster
    {
        public FireDragon(int strenght) : base("Dragon", strenght, ElementType.fire)
        {
            Console.WriteLine("Here comes Dragon");
        }
        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {

            return tmpDamage;
        }
    }
}
