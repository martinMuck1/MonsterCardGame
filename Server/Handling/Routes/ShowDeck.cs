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
    public class ShowDeck :Handler
    {
        public ShowDeck( AuthLevel level) :base(level)
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
            
            //get deck from user, get card Models from deck id and then send the results to user
            IDeckDao deckdao = new DeckDao();
            ICardDao cardao = new CardDao();
            UserModel modelUser = new UserModel(username);
            DeckModel modelDeck = deckdao.ShowDeckCards(DBHelper.ConvertNameToID(modelUser.Username));
            if(modelDeck == null)
            {
                Console.WriteLine("No cards in user deck");
                res.SendResponse(responseType.ERR, "message: no cards in your deck yet!");
                return;
            }
            List<CardModel> cardList = new List<CardModel>();
            foreach (var cardID in modelDeck.Card)
            {
                cardList.Add(cardao.ShowSingleCard(cardID));
            }
            JArray array = ListToJSON(cardList);
            Console.WriteLine("Sent User deck");
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
        }
    }
}
