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
    public class DeleteTrade :Handler
    {

        public DeleteTrade( AuthLevel level) :base(level)
        {
        }

        public override void DeserializeMessage(string message)
        {
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;
            string username = "";
            TradeModel modelTrade = null;
            if (!CheckOwnerTrade(token, out username, ref modelTrade))
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Cant delete Trade Offer => not yours or not in offers yet!\"}");
                return;
            }

            TradeDao tradedao = new TradeDao();
            if (tradedao.DeleteTrade(modelTrade) != 0)     
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not delete Trade offer!\"}");
                return;
            }
            res.SendResponse(responseType.OK, "{\"message\": \"deleted Trade offer successfully\"}");
            Console.WriteLine("deletion was successfull");
        }

        /*check if cardID from request belongs to the user */
        private  bool CheckOwnerTrade(string token, out string username,ref TradeModel model) 
        {
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return false;
            }
            TradeDao tradedao = new TradeDao();
            List<TradeModel> list = tradedao.ShowAllTradeOffers();
            if (this.Param == "" || !list.Select((x) => x.TID).Contains(this.Param)){       //request param is not in trade table
                Console.WriteLine("Requested Trade ID was not found!");
                return false;
            }
            var trade = list.Where((x) => x.TID == this.Param).First();
            model = new TradeModel(trade.TID, trade.CardToTrade, trade.CardType, trade.MinDamage, username);
            if (trade.UID != model.UID)         //different user created trade 
            {
                Console.WriteLine("Trade ID does not belong to the user!");
                return false;
            }
            return true;
        }
    }
}
