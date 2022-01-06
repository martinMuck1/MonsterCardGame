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
    public class PostCreatePackage :Handler
    {
        struct formatPackageCard
        {
            public string id;
            public string name;
            public float damage;
        }

        struct formatPackage
        {
            public formatPackageCard fpackage;
        }

        private List<formatPackageCard> _package;

        public PostCreatePackage( AuthLevel level) : base(level)
        {

        }

        public override void DeserializeMessage(string message)
        {
            _package = JsonConvert.DeserializeObject<List<formatPackageCard>>(message);
        }

        public override void Handle(Response res,string token)
        {
            int errCode;
            if (!CheckAuth(res, token))
                return;

            IPackageDao packdao = new PackageDao();
            ICardDao carddao = new CardDao();

            PackageModel package = new PackageModel(Guid.NewGuid().ToString());
            if ((errCode = packdao.CreatePackage(package)) != 0)        //create package id
            {
                if(errCode == -1)
                    res.SendResponse(responseType.ERR, "{\"message\": \"Package ID probably already exists\"}");
                return;
            }

            foreach (var card in _package)      //create cards for package
            {
                CardModel tmpCardObj = new CardModel(card.id, card.name, card.damage);
                if (carddao.CreateCard(tmpCardObj, package.PackageID) != 0)
                {
                    res.SendResponse(responseType.ERR, "{\"message\": \"at least one Card ID already exists\"}");
                    return;
                }
            }
            Console.WriteLine("Created Packages successfully");
            JObject obj = new JObject();
            obj["message"] = "created Package successfully";
            obj["packageID"] = package.PackageID;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
