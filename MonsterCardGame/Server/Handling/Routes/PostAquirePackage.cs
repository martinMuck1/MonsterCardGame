using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterCardGame.Database;
using Newtonsoft.Json.Linq;

namespace MonsterCardGame.Server
{
    class PostAquirePackage : Handler
    {

        public PostAquirePackage(AuthLevel level) : base(level)
        {

        }

        public override void DeserializeMessage(string message)
        {
            //request has no json input
        }

        public override void Handle(Response res, string token)
        {
            string username = "";
            if (!CheckAuth(res, token, ref username))
                return;
            int packageCost = 5;

            IUserDao userDao = new UserDao();
            UserModel user = new UserModel(username);

            //check balance of account 
            Console.WriteLine("Cheking Balance...");
            int tmpBalance = userDao.CheckAccountBalance(user.Username);
            if (tmpBalance < packageCost)
            {
                Console.WriteLine("Error: not enough coins for transaction");
                res.SendResponse(responseType.ERR, "{\"message\": \"user does not have enough balance to aquire package!\"}");
                return;
            }

            //aquire random package
            Console.WriteLine("Aquiring Package...");
            PackageModel aquiredPacakge =  SetPackagesOwner(username);
            if(aquiredPacakge == null)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"no packages available, please contact admin\"}");
                return;
            }

            //pay for package
            if(userDao.PayWithCoins(user.Username, packageCost) != 0 ) 
            {
                string message = "{\"message\": \"some error occured while trying to pay coins\"}";
                res.SendResponse(responseType.ERR, message);
                return;
            }

            //set entry in transaction history
            TransactionModel modelTra = new TransactionModel(username, -packageCost, aquiredPacakge.PackageID);
            if(userDao.InsertTransaction(modelTra) != 0)
            {
                string message = "{\"message\": \"some error occured while trying to set transaction\"}";
                res.SendResponse(responseType.ERR, message);
                return;
            }

            Console.WriteLine($"{username} aquired package { aquiredPacakge.PackageID}");
            JObject obj = new JObject();
            obj["message"] = "Aquired package successfully";
            obj["packageID"] = aquiredPacakge.PackageID.Trim();
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }

        //Function to aquire packages => in Cards DB owner and check field in Packages
        private PackageModel SetPackagesOwner(string username)
        {
            var random = HTTPServer.random;
            IPackageDao packageDao = new PackageDao();
            ICardDao cardao = new CardDao();
            List<PackageModel> packageIDList = packageDao.GetAllUnaquiredPackages();
            if (packageIDList.Count == 0)
            {
                Console.WriteLine("Error: No available package");
                return null;
            }
            int index = random.Next(packageIDList.Count);
            PackageModel aquiredPacakge = new PackageModel(packageIDList[index].PackageID);
            List<CardModel> cardList = cardao.showPackageCards(aquiredPacakge);     // all cards in package
            foreach (var card in cardList)          //add user data into model => for db input
            {
                card.Username = username;
                card.SetUID();
                if (cardao.ChangeCardsOwner(card) != 0)     //change owner in card table
                {
                    Console.WriteLine("changing card Owner went wrong");
                    return null;
                }
            }

            if (packageDao.AquirePackage(aquiredPacakge) != 0)      //mark check in package table => no further aquiring allowed
                return null;

            return aquiredPacakge;
        }
    }
}
