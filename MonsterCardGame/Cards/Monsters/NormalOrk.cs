using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    public class NormalOrk: AbstractMonster
    {
        public NormalOrk(int strenght):base("Ork", strenght, ElementType.normal)
        {
            //Console.WriteLine("Knight entered the room");
        }
        /*
        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {
            if (opponent is NormalWizzard)
            {
                Console.WriteLine($"{this.Name} got hypnothized by {opponent.Name}");
                return 0;
            }
            return tmpDamage;
        }
        */
    }
}
