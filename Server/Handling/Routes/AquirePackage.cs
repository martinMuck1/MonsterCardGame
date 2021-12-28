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
            int returnVal;
            var random = new Random();
            IUserDao userDao = new UserDao();
            UserModel user = new UserModel(HTTPServer.Username);

            //check balance of account and spend coins
            Console.WriteLine("Cheking Balance");
            if((returnVal = userDao.PayWithCoins(user.Username, 5)) != 0) 
            {
                string message = "{message: some error occured while trying to pay coins}";

                if( returnVal == -1)
                    message = "{message: updating coins failed}";
                if(returnVal == -2)
                    message = "{message: requesting coins from db failed}";
                if(returnVal == -3)
                    message = "{message: user does not have enough balance to aquire package!}";
                res.SendResponse(responseType.ERR, message);
                return;
            }

            //aquire random package
            Console.WriteLine("aquiring Package");
            IPackageDao packageDao = new PackageDao();
            List<PackageModel> packageIDList = packageDao.GetAllUnaquiredPackages();
            int index = random.Next(packageIDList.Count);
            PackageModel aquiredPacakge = new PackageModel(packageIDList[index].PackageID, HTTPServer.Username);
            aquiredPacakge.SetUID();
            if (packageDao.AquirePackage(aquiredPacakge) != 0)
            {
                res.SendResponse(responseType.ERR, "{message: could not aquire desired package, please contact admin}");
                return;
            }

            Console.WriteLine("sending response");
            JObject obj = new JObject();
            obj["message"] = "Aquired package successfully";
            obj["packageID"] = aquiredPacakge.PackageID;
            //obj["username"] = aquiredPacakge.Username;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
