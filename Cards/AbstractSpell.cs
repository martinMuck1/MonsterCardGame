using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class AbstractSpell: AbstractCard
    {
        public AbstractSpell(string name, int damage, ElementType element):base(name, damage, element)
        {

        }

        protected override int TestSpecialCases(int tmpDamage, AbstractCard opponent)
        {
            if (opponent is Kraken)
            {
                Console.WriteLine($"{this.Name} has no effect on {opponent.Name}");
                return 0;
            }
            return tmpDamage;
        }
    }
}
