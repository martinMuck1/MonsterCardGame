using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterCardGame.Database;
using Newtonsoft.Json.Linq;
using MonsterCardGame.Battle;
using MonsterCardGame.Cards;

namespace MonsterCardGame.Server
{
    public class PostTrade :Handler
    {
        formatTradeOffer _offer;
        string _tradeCard;
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
        public void DeserializeMessageAlternative(string message)
        {
            _tradeCard = JsonConvert.DeserializeObject<string>(message);
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;
            string username = "";
            int result;

            string cardID = (Param == "") ? _offer.CardToTrade : _tradeCard;
            if ((result = CheckOwnerCard(token, cardID, out username)) != 0)
            {
                if (result == -1)
                {
                    res.SendResponse(responseType.ERR, "{\"message\": \"This card does not belong to you!\"}");
                    Console.WriteLine("Card does not belong to user");
                }
                else
                {
                    res.SendResponse(responseType.ERR, "{\"message\": \"You cant trade card if its in your deck\"}");
                    Console.WriteLine("Card is in user Deck!");
                }
                return;
            }

            if (Param == "")
            {            
                CreateTradeOffer(res, username);
            }
            else
            {
                StartTrade(res, username);
            }

        }

        /*check if cardID from request belongs to the user */
        private  int CheckOwnerCard(string token, string cardID,out string username) 
        {
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return -1;
            }
            CardDao carddao = new CardDao();
            List<CardModel> cardList = carddao.ShowAquiredCards(DBHelper.ConvertNameToID(username));    //get cards of user
            var cardIDList = cardList.Select(x => x.CardID); 
            if (!cardIDList.Contains(cardID))
            {
                return -1;
            }

            //further check of card is in deck
            DeckDao deckdao = new DeckDao();
            DeckModel modelD = deckdao.ShowDeckCards(DBHelper.ConvertNameToID(username));
            if (modelD.Card.Contains(cardID))
                return -2;

            return 0;
        }

        private void CreateTradeOffer(Response res, string username)
        {
            string type = _offer.Type;
            int minDamage = _offer.MinimumDamage;
            TradeDao tradedao = new TradeDao();
            TradeModel modelTrade = new TradeModel(_offer.Id, _offer.CardToTrade, type, minDamage, username);
            if (tradedao.CreateTradeOffer(modelTrade) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not Insert Trade offer!\"}");
                return;
            }
            res.SendResponse(responseType.OK, "{\"message\": \"Created Offer successfully\"}");
            Console.WriteLine("Created Trade Offer");
        }

        private void StartTrade(Response res, string username)
        {
            TradeDao tradedao = new TradeDao();
            List<TradeModel> list = tradedao.ShowAllTradeOffers();
             //check trade ID
            if (!list.Select((x) => x.TID).Contains(this.Param))
            {      
                Console.WriteLine("Requested Trade ID was not found!");
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not find Trade ID\"}");
                return;
            }
            //check where request came from 
            var tradeModel = list.Where((x) => x.TID == this.Param).First();    //find offer trade id
            if (tradeModel.UID == DBHelper.ConvertNameToID(username))         //same user for trade offer and request 
            {
                Console.WriteLine("User tried to trade with himself!");
                res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"User tried to trade with himself\"}");
                return;
            }
            //check if card fullfillst requirements
            if (!CheckCardRequirements(tradeModel))
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Requirements for card are not fullfilled!\"}");
                return;
            }
            
            //create models with reversed users
            CardDao cardao = new CardDao();
            int uidOffer = tradeModel.UID;
            int uidRequest = DBHelper.ConvertNameToID(username);
            CardModel cardModelOffer = new CardModel(tradeModel.CardToTrade, uidRequest);
            CardModel cardModelRequest = new CardModel(_tradeCard, uidOffer);
            //fullfill trade in db
            if (cardao.ChangeCardsOwner(cardModelOffer) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not change owner of wanted card! Please contact admin\"}");
                return;
            }
            if (cardao.ChangeCardsOwner(cardModelRequest) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Could not change owner of your old card! Please contact admin\"}");
                return;
            }
            Console.WriteLine("Traded successfully");
            res.SendResponse(responseType.OK, "{\"message\": \"trade deal was successfull\"}");
            tradedao.DeleteTrade(tradeModel);
        }
        private bool CheckCardRequirements(TradeModel tModel)
        {
            CardDao cardao = new CardDao();
            CardModel tradeCard = cardao.ShowSingleCard(_tradeCard);

            if(tModel.MinDamage > (int)tradeCard.Damage)
            {
                Console.WriteLine("Trade Card damage not enough");
                return false;
            }

            //find type 
            AbstractCard cardObj = AbstractDeckManager.GetAbstractCard(tradeCard.Name, (int)tradeCard.Damage);
            switch (tModel.CardType.ToLower().Trim())
            {
                case "spell":
                    if (cardObj is not AbstractSpell)
                    {
                        Console.WriteLine("Trade Card should be Spell");
                        return false;
                    }
                    break;
                case "monster":
                    if (cardObj is not AbstractMonster)
                    {
                        Console.WriteLine("Trade Card should be monster");
                        return false;
                    }
                    break;
                case "any":
                    return true;
                default:
                    return false;
            }
            return true;
        }
    }
}
