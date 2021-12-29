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
    public class DefineDeck :Handler
    {
        private string[] _reqCard;

        public DefineDeck( AuthLevel level) :base(level)
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
                res.SendResponse(responseType.ERR, "{message: not enough card IDs submitted => need to be four}");
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
            /*      Check if cards belong to this user 
            List<CardModel> cardList = carddao.ShowAquiredCards(DBHelper.ConvertNameToID(modelUser.Username));

            foreach (var cardID in modelDeck.Card)
            {
                if (cardList.Contains(cardID))
                    break
            }
            var test2NotInTest1 = cardList.Where(t2 => modelDeck.Count(t1 => t2.Contains(t1)) == 0);
            */
            if (deckdao.AddCardsToDeck(modelDeck) != 0)
            {
                res.SendResponse(responseType.ERR, "{message: could not set cards in deck}");
                Console.WriteLine("Could not set cards in deck");
                return;
            }
            Console.WriteLine("added cards to deck");
            res.SendResponse(responseType.OK, "{message: added Cards to deck successfully}");
        }
    }
}
