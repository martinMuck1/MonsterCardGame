using MonsterCardGame.Cards;
using MonsterCardGame.Battle;
using MonsterCardGame.DeckManage;
using MonsterCardGame.Users;
using System;

namespace MonsterCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            User user1 = new User("Martin");
            User user2 = new User("Amila");

            Fight battle = new Fight(ref user1, ref user2);
            battle.startFight();





            /*
            Fight objFight = new Fight();
            AbstractCard card1 = new WaterGoblin(80);
            AbstractCard card2 = new FireDragon(100);
            objFight.RoundFight(ref card1, ref card2);
            */
        }
    }
}
