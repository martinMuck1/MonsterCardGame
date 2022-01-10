using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Database;
using Newtonsoft.Json.Linq;

namespace MonsterCardGame.Server
{
    public abstract class Handler : IHandler
    {
        protected AuthLevel _authLevel;
        public string Param { get; set; } = "";

        public Handler(AuthLevel lev)
        {
            this._authLevel = lev;
        }

        public bool CheckAuth(Response res, string token, ref string username)
        {
            if (_authLevel == AuthLevel.noLogin)
                return true;
            if (_authLevel == AuthLevel.Login)
            {
                if (!Session.SessionDic.ContainsKey(token))
                {
                    Console.WriteLine("No login fullfilled => not authorized to succeed with this request");
                    res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"access denied\"}");
                    return false;
                }
                username = GetUserName(token);
                if(username == "")
                {
                    Console.WriteLine("Username could not be found");
                    res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"access denied\"}");
                    return false;
                }
                //request token is in memory => permission accepted
                Console.WriteLine("Token accepted");
                return true;
            }
            if (_authLevel == AuthLevel.Admin)
            {
                if (!Session.SessionDic.ContainsKey(token))
                {
                    Console.WriteLine("No login fullfilled => not authorized to succeed with this request");
                    res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"access denied\"}");
                    return false;
                }

                if (token == "admin-mtcgToken")     //normally this should be persisted in db
                {
                    username = GetUserName(token);
                    Console.WriteLine("Admin Token accepted");
                    return true;
                }
            }
            res.SendResponse(responseType.UNAUTHORIZED, "{\"message\": \"access denied\"}");
            return false;
        }

        protected string GetUserName(string token)
        {
            string username;
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return "";
            }
            return username;
        }

        public abstract void Handle(Response res, string token);
        public abstract void DeserializeMessage(string message);

        //convert List of cards to json
        protected JArray ListToJSON(List<CardModel> cardList){
            JArray array = new JArray();
            foreach (var card in cardList)
            {
                JObject obj = new JObject();
                obj["ID"] = card.CardID;
                obj["Name"] = card.Name;
                obj["Damage"] = card.Damage;
                array.Add(obj);
            }
            return array;
        }

        //convert List of stats to JSON
        protected JObject StatsToJSON(ScoreModel score)
        {
            JObject obj = new JObject();
            obj["username"] = score.Username;
            obj["elo"] = score.Elo;
            obj["wins"] = score.Wins;
            obj["games"] = score.Games;
            obj["loses"] = score.Loses;
            obj["ratio"] = Session.CalculateRatio(score.Wins, score.Loses, score.Games).ToString("0.000");
            return obj;
        }
    }
}
