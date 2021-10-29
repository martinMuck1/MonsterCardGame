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
            objFight.RoundFight(new WaterGoblin(), new WaterGoblin());
        }
    }
}
