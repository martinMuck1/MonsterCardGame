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
    public class ShowCardsHandle :Handler
    {
        public ShowCardsHandle( AuthLevel level) :base(level)
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
          
            CardDao cardao = new CardDao();
            JArray array = new JArray();
            UserModel modelUser = new UserModel(username);
            List<CardModel> cardList = cardao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username));
            foreach (var card in cardList)
            {
                JObject obj = new JObject();
                obj["ID"] = card.CardID;
                obj["Name"] = card.Name;
                obj["Damage"] = card.Damage;
                array.Add(obj);
            }
            Console.WriteLine("Sent Aquired Packages to user");
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
        }
    }
}
