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
    public class PutDeck :Handler
    {
        private string[] _reqCard;

        public PutDeck( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {
            _reqCard = JsonConvert.DeserializeObject<string[]>(message);

        }

        public override void Handle(Response res,string token)
        {
            string username = "";
            if (!CheckAuth(res, token, ref username))
                return;
            if (_reqCard.Length != 4)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"not enough card IDs submitted => need to be four\"}");
                Console.WriteLine("Not enough Cards submitted for deck!");
                return;
            }
           
            DeckDao deckdao = new DeckDao();
            CardDao carddao = new CardDao();
            UserModel modelUser = new UserModel(username);
            DeckModel modelDeck = new DeckModel(DBHelper.ConvertNameToID(modelUser.Username), _reqCard[0], _reqCard[1], _reqCard[2], _reqCard[3]);

            //check if any card is in trade offer
            TradeDao tradedao = new TradeDao();
            List<TradeModel> modelTList = tradedao.ShowAllTradeOffers();
            var modelIDList = modelTList.Select((x) => x.CardToTrade);
            var modelListTradeCards = modelDeck.Card.Where(w => modelIDList.Contains(w)).ToList();
            if(modelListTradeCards.Count() > 0)
            {
                Console.WriteLine("Desired Deck cards are in a trade offer");
                res.SendResponse(responseType.ERR, "{\"message\": \"Your desired deck cards are in a tradeoffer!\"}");
                return;
            }

            // Check if cards belong to this user 
            List<CardModel> cardList = carddao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username));
            var cardIDList = cardList.Select(x => x.CardID);
            var test2NotInTest1 = modelDeck.Card.Where(w => cardIDList.Contains(w)).ToList();       //check if cards of deck are in aquired cards
            if (test2NotInTest1.Count() != 4)
            {
                Console.WriteLine("User tried to put cards into deck which dont belong to him/her!");
                res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"Some cardIDs dont belong to you!\"}");
                return;
            }
            if (deckdao.ShowDeckCards(DBHelper.ConvertNameToID(modelUser.Username)) != null)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Your deck is already defined!\"}");
                Console.WriteLine("User Deck already set!");
                return;
            }

            if (deckdao.AddCardsToDeck(modelDeck) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"could not set cards in deck\"}");
                Console.WriteLine("Could not set cards in deck");
                return;
            }
            Console.WriteLine("added cards to deck");
            res.SendResponse(responseType.OK, "{\"message\": \"added Cards to deck successfully\"}");
        }
    }
}
