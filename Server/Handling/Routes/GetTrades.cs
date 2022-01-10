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
    public class GetTrades :Handler
    {
        public GetTrades( AuthLevel level) :base(level)
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

            TradeDao tradedao = new TradeDao();
            List<TradeModel> modelTradeList = tradedao.ShowAllTradeOffers();
            JArray array = new JArray();
            if (modelTradeList.Any())
            {
                foreach (var trade in modelTradeList)
                {
                    JObject obj = new JObject();
                    string user = "";
                    obj["tradeID"] = trade.TID;
                    obj["cardID"] = trade.CardToTrade;
                    obj["cardType"] = trade.CardType;
                    obj["minDamage"] = trade.MinDamage;
                    Session.SetUserDic();
                    if (!Session.UserDic.TryGetValue(trade.UID, out user))
                    {
                        Console.WriteLine("User unkown");
                        array.Add(obj);
                    }
                    obj["user"] = user;
                    array.Add(obj);
                }
            }
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
            Console.WriteLine("Sent all Trades to user");
        }
    }
}
