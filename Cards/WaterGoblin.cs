using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards
{
    class WaterGoblin: AbstractMonster
    {
        public  WaterGoblin():base("Goblin", 100, ElementType.normal)
        {
            Console.WriteLine("Here comes WaterGoblin");
        }
        protected override int AdaptDamageMonsterOpponent(AbstractCard opponent)
        {
            //here comes special behaviour for other monsters
            return this.Damage;
        }

    }
}
