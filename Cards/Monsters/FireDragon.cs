using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class FireDragon : AbstractMonster
    {
        public FireDragon(int strenght) : base("Dragon", strenght, ElementType.fire)
        {
        }
        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {
            if(opponent is FireElves)
            {
                Console.WriteLine($"{opponent.Name} know all tricks of {this.Name} and could manage to dodge attack!");
                return 0;
            }
            return tmpDamage;
        }
    }
}
