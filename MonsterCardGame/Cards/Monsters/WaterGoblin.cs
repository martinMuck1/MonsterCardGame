using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class WaterGoblin: AbstractMonster
    {
        public  WaterGoblin(int strenght) :base("Goblin",strenght, ElementType.water)
        {
            //Console.WriteLine("Here comes WaterGoblin");
        }
        /*
        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {
            if (opponent is FireDragon)
            {
                Console.WriteLine($"{this.Name} is to aftraid to attack {opponent.Name} !");
                return 0;
            }
            return tmpDamage;
        }
        */
    }
}
