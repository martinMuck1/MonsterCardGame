using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface ITradeDao
    {
        public List<TradeModel> ShowAllTradeOffers();
        public int CreateTradeOffer(TradeModel model);
        public int DeleteTrade(TradeModel model);
    }
}
