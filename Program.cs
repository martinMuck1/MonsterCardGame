using MonsterCardGame.Cards;
using MonsterCardGame.Battle;
using MonsterCardGame.DeckManage;
using System;

namespace MonsterCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //should be a battle between players
            Fight objFight = new Fight();
            AbstractCard card1 = new WaterGoblin(80);
            AbstractCard card2 = new FireDragon(100);
            objFight.RoundFight(ref card1, ref card2);
            card1 = new WaterSpell(80);
            card2 = new Kraken(100);
            objFight.RoundFight(ref card1, ref card2);
            card1 = new WaterSpell(80);
            card2 = new FireDragon(100);
            objFight.RoundFight(ref card1, ref card2);
            card1 = new WaterSpell(80);
            card2 = new WaterSpell(80);
            objFight.RoundFight(ref card1, ref card2);
        }
    }
}
