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
            if (!CheckAuth(res, token))
                return;
            if (_reqCard.Length != 4)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"not enough card IDs submitted => need to be four\"}");
                Console.WriteLine("Not enough Cards submitted for deck!");
                return;
            }
            string username;
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return;
            }
           
            DeckDao deckdao = new DeckDao();
            CardDao carddao = new CardDao();
            UserModel modelUser = new UserModel(username);
            DeckModel modelDeck = new DeckModel(DBHelper.ConvertNameToID(modelUser.Username), _reqCard[0], _reqCard[1], _reqCard[2], _reqCard[3]);

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
