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
    public enum Player
    {
        playerA,
        playerB,
        none
    }
     public struct SpecialRound
    {
        public SpecialRound(bool valActive)
        {
            Activated = valActive;
            Player = Player.none;
            Round = 100;
        }
        public void Reset()
        {
            Activated = false;
            Round = 101;
            Player = Player.none;
        }
        public bool Activated { get; set; }
        public Player Player { get; set; }
        public int Round { get; set; }
    }

    public class Fight
    {
        Players _playerA;
        Players _playerB;
        static SpecialRound _specialRound = new SpecialRound(false);
        static bool _specialR = false;

        public Fight(Players playerA, Players playerB)
        {
            this._playerA = playerA;
            this._playerB = playerB;
            _specialRound.Activated = true;     //only when object is created 
        }

        public Outcome startFight()        //whole fight of two players with their decks
        {
            Outcome endresult = Outcome.deuce;
            AbstractDeckManager deckA= _playerA.MyDeck, deckB = _playerB.MyDeck;
            int tmpResult = 0;
            //for demonstration purpose
            Console.WriteLine("-------- Start Fight---------");
            Console.WriteLine($"Player A({_playerA.Name}): ");
            deckA.showAllCards();
            Console.WriteLine($"Player B({_playerB.Name}): ");
            deckB.showAllCards();
            Console.WriteLine();

            if(_specialRound.Activated == true)
            {
                _specialRound.Round = 5 + HTTPServer.random.Next(6);       //boost in round 5,...,10
            }
            //get count of cards from each player
            //play one round with random cards => loser loses card
            //first player with no cards loses game
            for (int rounds = 0; rounds < 100; rounds++)         
            {
                if (rounds == _specialRound.Round)
                    PrepareSpecialFeature(deckA.getSizeStack(), deckB.getSizeStack()); 

                if (((tmpResult = PlayOneRound(deckA, deckB)) != 0) || (rounds == 99 && tmpResult == 0))
                {
                    switch (tmpResult)
                    {
                        case 0:
                            Console.WriteLine($"no winner!!");
                            endresult = Outcome.deuce;
                            break;
                        case -1:
                            Console.WriteLine($"{_playerB.Name} won the battle!");
                            endresult = Outcome.winnerB;
                            break;
                        case -2:
                            Console.WriteLine($"{_playerA.Name} won the battle!");
                            endresult = Outcome.winnerA;
                            break;
                        default:
                            break;
                    }
                    _specialR = false;
                    _specialRound = new SpecialRound(false);
                    break;
                }
            }
            Console.WriteLine("-------- End Figth---------");
            return endresult;
        }

        public static int PlayOneRound(AbstractDeckManager deckA, AbstractDeckManager deckB)
        {
            Outcome tmpResult;
            Random rnd = HTTPServer.random;
            int deckNrA, deckNrB;

            int countA = deckA.getSizeStack();
            deckNrA = rnd.Next(countA);
            AbstractCard cardA = deckA.showCard(deckNrA);

            int countB = deckB.getSizeStack();
            deckNrB = rnd.Next(countB);
            AbstractCard cardB = deckB.showCard(deckNrB);

            tmpResult = RoundFight(cardA, cardB);
            if (tmpResult == Outcome.winnerB)
            {
                deckA.RemoveCard(deckNrA);
                deckB.AddCard(cardA);
                if (countA == 1)        //last card count from opponent was 1 + he lost again = loser
                { 
                    return -1;
                }
            }

            if (tmpResult == Outcome.winnerA)
            {
                deckB.RemoveCard(deckNrB);
                deckA.AddCard(cardB);
                if (countB == 1)
                {
                    return -2;
                }
            }
            return 0;
        }

        //round = compare adapted damage of cards
        public static Outcome RoundFight( AbstractCard cardA, AbstractCard cardB)    
        {
            Outcome tmpOutcome;
            int tmpDamageCardA = cardA.AdaptDamage(cardB);
            int tmpDamageCardB = cardB.AdaptDamage(cardA);

            if (_specialR)
                PrepareSpecialFeature(ref tmpDamageCardA, ref tmpDamageCardB);

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
                Console.WriteLine($"No winners this round => Player A: {cardA.Name} ({cardA.Damage} damage) against Player B: {cardB.Name} ({cardB.Damage} damage) => same value:  {tmpDamageCardA}");
            }

            return tmpOutcome;
        }

        private static void PrepareSpecialFeature(int deckASize, int deckBSize)
        {
            _specialR = true;
            if (deckASize > deckBSize)
            {
                _specialRound.Player = Player.playerB;
            }else if (deckASize < deckBSize)
            {
                _specialRound.Player = Player.playerA;
            }
            else
            {
                Player[] arr = { Player.playerA, Player.playerB };
                _specialRound.Player = arr[HTTPServer.random.Next(arr.Length)];
            }
        }

        private static void PrepareSpecialFeature(ref int tmpDamageCardA, ref int tmpDamageCardB)
        {
            int multiply = HTTPServer.random.Next(2, 5);
            if(_specialRound.Player == Player.playerA)
            {
                tmpDamageCardA *= multiply;
            }
            else
            {
                tmpDamageCardB *= multiply;
            }
            Console.WriteLine($"----------Booster activated vor this round! => {_specialRound.Player.ToString()} card is {multiply}x higher -----------");
            _specialRound.Reset();
            _specialR = false;
        }
    }
}
