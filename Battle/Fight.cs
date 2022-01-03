using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Cards;
using MonsterCardGame.Server;

namespace MonsterCardGame.Battle
{
    public enum Outcome
    {
        winnerA,
        winnerB,
        deuce
    }
    public class Fight
    {
        Players _playerA;
        Players _playerB;

        public Fight(Players playerA, Players playerB)
        {
            this._playerA = playerA;
            this._playerB = playerB;
        }

        public Outcome startFight()        //whole fight of two players with their decks
        {
            Outcome tmpResult, endresult = Outcome.deuce;
            Random rnd = HTTPServer.random;
            int deckNrA, deckNrB;
            AbstractDeckManager deckA= _playerA.MyDeck, deckB = _playerB.MyDeck;

            //for demonstration purpose
            Console.WriteLine("-------- Start Figth---------");
            Console.WriteLine($"Player A({_playerA.Name}): ");
            deckA.showAllCards();
            Console.WriteLine($"Player B({_playerB.Name}): ");
            deckB.showAllCards();
            Console.WriteLine();

            //get count of cards from each player
            //play one round with random cards => loser loses card
            //first player with no cards loses game
            for (int rounds = 0; rounds < 100; rounds++)         
            {
                int countA = deckA.getSizeStack();
                deckNrA = rnd.Next(countA);
                AbstractCard cardA = deckA.showCard(deckNrA);

                int countB = deckB.getSizeStack();
                deckNrB = rnd.Next(countB);
                AbstractCard cardB = deckB.showCard(deckNrB);
  
                tmpResult = RoundFight( cardA, cardB);
                if(tmpResult == Outcome.winnerB)
                {
                    deckA.RemoveCard(deckNrA);
                    deckB.AddCard(cardA);
                    if (countA == 1)        //last card count from opponent was 1 + he lost again = loser
                    {
                        Console.WriteLine($"{_playerB.Name} won the battle!");
                        endresult = Outcome.winnerB;
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
                        endresult = Outcome.winnerA;
                        break;
                    }
                }
            }
            Console.WriteLine("-------- End Figth---------");
            return endresult;
        }

        //round = compare adapted damage of cards
        public static Outcome RoundFight( AbstractCard cardA, AbstractCard cardB)    
        {
            Outcome tmpOutcome;
            int tmpDamageCardA = cardA.AdaptDamage(cardB);
            int tmpDamageCardB = cardB.AdaptDamage(cardA);

            if (tmpDamageCardA > tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerA;
                Console.WriteLine($"Round Winner = Player A: {cardA.Name} ({cardA.Damage} damage) against Player B: {cardB.Name} ({cardB.Damage} damage) => {tmpDamageCardA} vs {tmpDamageCardB}");
            }
            else if (tmpDamageCardA < tmpDamageCardB)
            {
                tmpOutcome = Outcome.winnerB;
                Console.WriteLine($"Round Winner = Player B: {cardB.Name} ({cardB.Damage} damage) against Player A: {cardA.Name} ({cardA.Damage} damage)=> {tmpDamageCardB} vs {tmpDamageCardA}");
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
