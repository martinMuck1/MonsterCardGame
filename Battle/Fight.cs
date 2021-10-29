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

        public Outcome RoundFight(AbstractCard cardA, AbstractCard cardB)
        {
            Outcome tmpOutcome;
            if(cardA.AdaptDamage(cardB) > cardB.AdaptDamage(cardA))
            {
                tmpOutcome = Outcome.winnerA;
                Console.WriteLine($"Winner is: {0}", cardA.Name);

            }
            else if (cardA.AdaptDamage(cardB) < cardB.AdaptDamage(cardA)){
                tmpOutcome = Outcome.winnerB;
                Console.WriteLine($"Winner is: {0}", cardB.Name);

            }
            else
            {
                tmpOutcome = Outcome.deuce;
                Console.WriteLine("No winners today");
            }

            return tmpOutcome;
        }
    }
}
