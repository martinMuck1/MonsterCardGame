using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;
using MonsterCardGame.Users;
using MonsterCardGame.DeckManage;

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
        User _playerA;
        User _playerB;

        public Fight(ref User playerA, ref User playerB)
        {
            this._playerA = playerA;
            this._playerB = playerB;
        }

        public void startFight()
        {
            //int winsA = 0, winsB = 0;
            Outcome tmpResult;
            Random rnd = new Random();
            int deckNrA, deckNrB;
            AbstractDeckManager deckA= _playerA.myDeck, deckB = _playerB.myDeck;

            for (int rounds = 0; rounds < 100; rounds++)
            {
                //Console.WriteLine("Round: {0}", rounds + 1);
                int countA = deckA.getSizeStack();
                deckNrA = rnd.Next(countA);
                AbstractCard cardA = deckA.showCard(deckNrA);

                int countB = deckB.getSizeStack();
                deckNrB = rnd.Next(countB);
                AbstractCard cardB = deckB.showCard(deckNrB);
                //deckA.showAllCards();
                //Console.WriteLine("Other deck:");
                //deckB.showAllCards();
                tmpResult = RoundFight(ref cardA, ref cardB);
                if(tmpResult == Outcome.winnerB)
                {
                    deckA.RemoveCard(deckNrA);
                    deckB.AddCard(cardA);
                    if (countA == 1)
                    {
                        Console.WriteLine($"{_playerB.Name} won the battle!");
                        break;
                    }
                }

                if (tmpResult == Outcome.winnerA)
                {
                    deckB.RemoveCard(deckNrB);
                    deckA.AddCard(cardB);
                    if (countB == 1)
                    {
                        Console.WriteLine($"{_playerA.Name} won the battle!");
                        break;
                    }
                }
            }
        }

        public Outcome RoundFight(ref AbstractCard cardA,ref AbstractCard cardB)
        {
            Outcome tmpOutcome;
            int tmpDamageCardA = cardA.AdaptDamage(cardB);
            int tmpDamageCardB = cardB.AdaptDamage(cardA);

            if (tmpDamageCardA > tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerA;
                Console.WriteLine($"Round Winner = Player A: {cardA.Name} ({tmpDamageCardA} damage) against Player B: {cardB.Name} ({tmpDamageCardB} damage)");
            }
            else if (tmpDamageCardA < tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerB;
                Console.WriteLine($"Round Winner = Player B: {cardB.Name} ({tmpDamageCardB} damage) against Player A: {cardA.Name} ({tmpDamageCardA} damage)");
            }
            else
            {
                tmpOutcome = Outcome.deuce;
                Console.WriteLine($"No winners this round => Damage:  {tmpDamageCardA}");
            }

            return tmpOutcome;
        }
    }
}
