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
    public class PostBattle :Handler
    {
        private BattleModel _userReq;

        public PostBattle( AuthLevel level) :base(level)
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

            ScoreDao scoredao = new ScoreDao();
            BattleModel modelBattleOpp;
            BattleModel modelBattlePlayer = new BattleModel(username);
            Console.WriteLine("Searching for opponent...");
            if((modelBattleOpp = scoredao.ShowRequests()) == null)      // no requests in table 
            {
                if(scoredao.CreateBattleRequest(modelBattlePlayer) != 0)
                {
                    Console.WriteLine("Could not create new Battle request");
                    res.SendResponse(responseType.ERR, "{\"message\": \"Couldnt create battle request\"}");
                    return;
                }
                Console.WriteLine("created battle request successfully");
                res.SendResponse(responseType.OK, "{\"message\": \"created battle request successfully\"}");
                return;
            }
            //request is already in db => start fight
            _userReq = modelBattleOpp;
            if(modelBattlePlayer.UID == modelBattleOpp.UID)
            {
                Console.WriteLine("User is trying to play agains himself");
                res.SendResponse(responseType.ERR, "{\"message\": \"you cant play against yourself\"}");
                return;
            }
            Players playerA = ConvertModelToPlayer(modelBattlePlayer);
            Players playerB = ConvertModelToPlayer(modelBattleOpp);
            if (playerA == null || playerB == null)
            {
                Console.WriteLine("No deck defined");
                res.SendResponse(responseType.ERR, "{\"message\": \"there is no deck defined for the players\"}");
                return;
            }
            StartBattle(playerA, playerB, res);
        }
        //Modeldata => get Player Object with deck for figth logic
        private Players ConvertModelToPlayer(BattleModel modelBattle)
        {
            //get decks from users
            DeckDao deckdao = new DeckDao();
            DeckModel modelDeckPlayer = deckdao.ShowDeckCards(modelBattle.UID);
            if (modelDeckPlayer == null)
                return null;
            List<CardModel> cardListPlayer = GetCardModelList(modelDeckPlayer);

            //set playerobject (username is not directly known from table)
            Session.SetUserDic();
            int uid = modelDeckPlayer.UID;
            string name;
            name = (!Session.UserDic.ContainsKey(uid)) ? "player": Session.UserDic[uid];
            Players player = new Players(name);

            //set player deck => create new list of cards and attach them to deck. at the end add deck to player
            List<AbstractCard> tmpCardList = new List<AbstractCard>();
            cardListPlayer.ForEach((cardModel)=> 
                tmpCardList.Add(AbstractDeckManager.GetAbstractCard(cardModel.Name, (int)cardModel.Damage)));
            AbstractDeckManager deck = new Deck(tmpCardList);
            player.MyDeck = deck;

            return player;
        }

        private List<CardModel> GetCardModelList(DeckModel modelDeck)
        {
            List<CardModel> tmpList = new List<CardModel>();
            CardDao cardao = new CardDao();
            foreach (var cardID in modelDeck.Card)
            {
                tmpList.Add(cardao.ShowSingleCard(cardID));
            }
            return tmpList;
        }

        private void StartBattle(Players playerA, Players playerB, Response res)
        {
            Fight fightObj = new Fight(playerA, playerB);
            Outcome result = fightObj.startFight();
            ScoreModel scoreModelA = new ScoreModel(playerA.Name), scoreModelB = new ScoreModel(playerB.Name);
            int eloWin = 3, eloLose = -5;
            switch (result)
            {
                case Outcome.winnerA:
                    scoreModelA.ScoreModelOffset(eloWin, 1, 1, 0);
                    scoreModelB.ScoreModelOffset(eloLose, 0, 1, 1);
                    break;
                case Outcome.winnerB:
                    scoreModelA.ScoreModelOffset(eloLose, 0, 1, 1);
                    scoreModelB.ScoreModelOffset(eloWin, 1, 1, 0);
                    break;
                case Outcome.deuce:
                    scoreModelA.ScoreModelOffset(0, 0, 1, 0);
                    scoreModelB.ScoreModelOffset(0, 0, 1, 0);
                    break;
                default:
                    break;
            }
            ScoreDao scoredao = new ScoreDao();
            if(scoredao.UpdateScoreBoard(scoreModelA) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Player A could not be updated\"}");
                return;
            }
            if (scoredao.UpdateScoreBoard(scoreModelB) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Player B could not be updated\"}");
                return;
            }
            res.SendResponse(responseType.OK, "{\"message\": \"fight finished, please take a look at your stats\"}");
            scoredao.DeleteBattleRequest(_userReq);
        }
    }
}
