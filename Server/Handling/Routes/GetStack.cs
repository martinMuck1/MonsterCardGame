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
    public class GetStack :Handler
    {
        public GetStack( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {

        }

        public override void Handle(Response res,string token)
        {
            string username = "";
            if (!CheckAuth(res, token, ref username))
                return;

            ICardDao cardao = new CardDao();
            UserModel modelUser = new UserModel(username);
            List<CardModel> cardList = cardao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username));
            JArray array = ListToJSON(cardao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username)));
            Console.WriteLine("Sent Aquired Packages = Stack to user");
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
        }
    }
}
