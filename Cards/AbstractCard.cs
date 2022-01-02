using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Cards 
{
    public enum ElementType
    {
        water,
        fire,
        normal 
    }
    public abstract class AbstractCard : ICard
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }     //damage cant be changed
        public ElementType Element { get; private set; }

        protected AbstractCard(string name, int damage, ElementType element)
        {
            this.Name = name;
            this.Damage = damage;
            this.Element = element;
        }
        public int AdaptDamage(AbstractCard opponent)   // calculate new damage, depending on opponent
        {
            int tmpDamage;
            if(this is AbstractMonster)
            {
                if (opponent is AbstractMonster) //Compare Monster to Monster
                {
                    tmpDamage = this.Damage;
                }
                else                            //Compare Monster to Spell
                {
                    tmpDamage = AdaptDamageSpellInvolved(opponent);
                }
            }
            else
            {
                tmpDamage = AdaptDamageSpellInvolved(opponent);     //spell vs spell or monster
            }
            //special cases like Kraken vs Spell etc.
            tmpDamage = TestSpecialCases(tmpDamage, opponent);      
            return tmpDamage;
        }

        protected virtual int TestSpecialCases(int tmpDamage, AbstractCard opponent)    //base version => returns damage
        {
            return tmpDamage;
        }

        protected int AdaptDamageSpellInvolved(AbstractCard opponent)  //logic for Element bonus
        {
            int tmpDamage = this.Damage;
            if(this.Element == ElementType.fire)
            {
                if(opponent.Element == ElementType.normal)
                {
                    tmpDamage += tmpDamage;
                }
                if(opponent.Element == ElementType.water)
                {
                    tmpDamage -= tmpDamage/2;
                }
            }

            if (this.Element == ElementType.water)
            {
                if (opponent.Element == ElementType.fire)
                {
                    tmpDamage += tmpDamage;
                }
                if (opponent.Element == ElementType.normal)
                {
                    tmpDamage -= tmpDamage / 2;
                }
            }
            if (this.Element == ElementType.normal)
            {
                if (opponent.Element == ElementType.water)
                {
                    tmpDamage += tmpDamage;
                }
                if (opponent.Element == ElementType.fire)
                {
                    tmpDamage -= tmpDamage / 2;
                }
            }

            return tmpDamage;
        }

    }
}
