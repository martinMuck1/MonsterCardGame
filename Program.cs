using MonsterCardGame.Battle;
using MonsterCardGame.Cards;
using MonsterCardGame.Users;
using MonsterCardGame.Server;
using System.Threading.Tasks;


//nur TCP listener (kein HTTP)
namespace MonsterCardGame
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HTTPServer myTCPconn = new HTTPServer(8000);
            await myTCPconn.StartServerAsync();
            
            /*
            User user1 = new User("Martin");
            User user2 = new User("Amila");

            Fight battle = new Fight(ref user1, ref user2);
            battle.startFight();
            */
            /*
            AbstractCard card1 = new WaterGoblin(120);
            AbstractCard card2 = new FireDragon(100);
            card1.AdaptDamage(card2);
            */
            /*
            AbstractCard card1 = new NormalKnight(80);
            AbstractCard card2 = new WaterSpell(100);
            card1.AdaptDamage(card2);
            */
            /*
            AbstractCard card1 = new WaterKraken(50);
            AbstractCard card2 = new NormalSpell(100);
            card1.AdaptDamage(card2);
            */

        }
    }
}
