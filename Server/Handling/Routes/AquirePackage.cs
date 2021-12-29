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
    class AquirePackage : Handler
    {

        public AquirePackage(AuthLevel level) : base(level)
        {

        }

        public override void DeserializeMessage(string message)
        {
            //request has no json input
        }

        public override void Handle(Response res, string token)
        {
            if (!CheckAuth(res, token))
                return;
            int packageCost = 5;
            var random = HTTPServer.random;
            string username;
            if(!Session.SessionDic.TryGetValue(token,out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return;
            }
            IUserDao userDao = new UserDao();
            UserModel user = new UserModel(username);

            //check balance of account 
            Console.WriteLine("Cheking Balance...");
            int tmpBalance = userDao.CheckAccountBalance(user.Username);
            if (tmpBalance < packageCost)
            {
                Console.WriteLine("Error: not enough coins for transaction");
                res.SendResponse(responseType.ERR, "{message: user does not have enough balance to aquire package!}");
                return;
            }

            //aquire random package
            Console.WriteLine("Aquiring Package...");
            IPackageDao packageDao = new PackageDao();
            List<PackageModel> packageIDList = packageDao.GetAllUnaquiredPackages();
            if(packageIDList.Count == 0)
            {
                Console.WriteLine("Error: No available package");
                res.SendResponse(responseType.ERR, "{message: no packages available, please contact admin}");
                return;
            }
            int index = random.Next(packageIDList.Count);
            PackageModel aquiredPacakge = new PackageModel(packageIDList[index].PackageID, username);
            aquiredPacakge.SetUID();
            if (packageDao.AquirePackage(aquiredPacakge) != 0)
            {
                res.SendResponse(responseType.ERR, "{message: could not aquire desired package, please contact admin}");
                return;
            }

            if(userDao.PayWithCoins(user.Username, 5) != 0) 
            {
                string message = "{message: some error occured while trying to pay coins}";
                res.SendResponse(responseType.ERR, message);
                return;
            }
            Console.WriteLine($"{aquiredPacakge.Username} aquired package { aquiredPacakge.PackageID}");
            JObject obj = new JObject();
            obj["message"] = "Aquired package successfully";
            obj["packageID"] = aquiredPacakge.PackageID;
            //obj["username"] = aquiredPacakge.Username;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
