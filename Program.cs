using MonsterCardGame.Battle;
using MonsterCardGame.Cards;
using MonsterCardGame.Server;
using System.Threading.Tasks;


//nur TCP listener (kein HTTP)
namespace MonsterCardGame
{
    
    class Program
    {
        static void Main(string[] args)
        {
            HTTPServer myTCPconn = new HTTPServer(10001);
            //Task task1 = Task.Factory.StartNew(async() => await myTCPconn.StartServerAsync());
            myTCPconn.StartServer();
        }
    }
}
