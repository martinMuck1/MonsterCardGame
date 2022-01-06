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
    public class GetDeck :Handler
    {
        public GetDeck( AuthLevel level) :base(level)
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
                res.SendResponse(responseType.OK, "[]");
                return;
            }
            List<CardModel> cardList = new List<CardModel>();
            foreach (var cardID in modelDeck.Card)
            {
                cardList.Add(cardao.ShowSingleCard(cardID));
            }

            if(this.Param == "format=plain")        // get request with special format
            {
                res.SetResponseTypeToPlain();
                var stringList = cardList.Select(x =>string.Join(",", x.CardID, x.Name.Trim(), x.Damage));
                res.SendResponse(responseType.OK, string.Join("\n", stringList.ToArray()));
            }
            else
            {   //json output
                JArray array = ListToJSON(cardList);
                Console.WriteLine("Sent User deck");
                res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
            }
        }
    }
}
