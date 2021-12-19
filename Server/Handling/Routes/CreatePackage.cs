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
    public class CreatePackage :Handler
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

        public CreatePackage( AuthLevel level) : base(level)
        {

        }

        public override void DeserializeMessage(string message)
        {
            _package = JsonConvert.DeserializeObject<List<formatPackageCard>>(message);
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            IPackageDao packdao = new PackageDao();
            PackageModel package = new PackageModel(Guid.NewGuid().ToString());
            _package.ForEach((card) => package.AddCard(card.id, card.name, card.damage));

            _package.ForEach((card) => Console.WriteLine(card.id + " "+card.name+" "+card.damage));

            if (packdao.CreatePackage(package) != 0)
            {
                res.SendResponse(responseType.ERR, "{message: Package ID probably already exists}");
                return;
            }

            JObject obj = new JObject();
            obj["message"] = "created Package successfully";
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
