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
    public class ShowStack :Handler
    {
        public ShowStack( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {

        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            string username;
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return;
            }
          
            ICardDao cardao = new CardDao();
            UserModel modelUser = new UserModel(username);
            List<CardModel> cardList = cardao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username));
            JArray array = ListToJSON(cardao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username)));
            Console.WriteLine("Sent Aquired Packages = Stack to user");
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
        }
    }
}
