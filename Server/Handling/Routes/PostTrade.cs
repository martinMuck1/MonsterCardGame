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
    public class PostTrade :Handler
    {
        formatTradeOffer _offer;
        struct formatTradeOffer
        {
            public string Id;
            public string CardToTrade;
            public string Type;
            public int MinimumDamage;
        }
        public PostTrade( AuthLevel level) :base(level)
        {
        }

        public override void DeserializeMessage(string message)
        {
            _offer = JsonConvert.DeserializeObject<formatTradeOffer>(message);
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;
            string username = "";
            if (!CheckOwnerCard(token, _offer.CardToTrade, out username))
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"This card does not belong to you!\"}");
                Console.WriteLine("Card does not belong to user");
                return;
            }

            string type = _offer.Type;
            int minDamage = _offer.MinimumDamage;
            TradeDao tradedao = new TradeDao();
            TradeModel modelTrade = new TradeModel(_offer.Id,_offer.CardToTrade, type, minDamage, username);
            if (tradedao.CreateTradeOffer(modelTrade) != 0)     
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not Insert Trade offer!\"}");
                return;
            }
            res.SendResponse(responseType.OK, "{\"message\": \"Created Offer successfully\"}");
            Console.WriteLine("Created Trade Offer");
        }

        /*check if cardID from request belongs to the user */
        private  bool CheckOwnerCard(string token, string cardID,out string username) 
        {
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return false;
            }
            CardDao carddao = new CardDao();
            List<CardModel> cardList = carddao.ShowAquiredCards(DBHelper.ConvertNameToID(username));    //get cards of user
            var cardIDList = cardList.Select(x => x.CardID); 
            if (!cardIDList.Contains(cardID))
            {
                return false;
            }
            return true;
        }
    }
}
