using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;

namespace MonsterCardGame.Battle
{
    enum Outcome
    {
        winnerA,
        winnerB,
        deuce
    }
    class Fight
    {

        public Fight()
        {
            //initialize Decks

        }

        public Outcome RoundFight(ref AbstractCard cardA,ref AbstractCard cardB)
        {
            Outcome tmpOutcome;
            int tmpDamageCardA = cardA.AdaptDamage(cardB);
            int tmpDamageCardB = cardB.AdaptDamage(cardA);

            if (tmpDamageCardA > tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerA;
                Console.WriteLine($"Winner is: {cardA.Name} with {tmpDamageCardA} against {tmpDamageCardB}");
            }
            else if (tmpDamageCardA < tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerB;
                Console.WriteLine($"Winner is: {cardB.Name} with {tmpDamageCardB} against {tmpDamageCardA}");
            }
            else
            {
                tmpOutcome = Outcome.deuce;
                Console.WriteLine($"No winners today => Damage:  {tmpDamageCardA}");
            }

            return tmpOutcome;
        }
    }
}
