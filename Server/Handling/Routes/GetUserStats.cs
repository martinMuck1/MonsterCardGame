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
    public class GetUserStats :Handler
    {
        public GetUserStats( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {

        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            string username;
            if (!Session.SessionDic.TryGetValue(token, out username))
            {
                //key is not in dic => should not happen cause of checkauth
                Console.WriteLine("Key not in Dictionary");
                return;
            }
          
            IScoreDao scoredao = new ScoreDao();
            ScoreModel scoreModel = new ScoreModel(username);
            JObject obj = new JObject();
            scoreModel = scoredao.ShowUserStats(scoreModel.UID);
            obj["username"] = username;
            obj["elo"] = scoreModel.Elo;
            obj["wins"] = scoreModel.Wins;
            obj["games"] = scoreModel.Games;
            obj["loses"] = scoreModel.Loses;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
            Console.WriteLine("Sent User Stats");
        }
    }
}
