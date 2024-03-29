﻿using System;
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
            string username = "";
            if (!CheckAuth(res, token, ref username))
                return;
          
            IScoreDao scoredao = new ScoreDao();
            ScoreModel scoreModel = new ScoreModel(username);
            scoreModel = scoredao.ShowUserStats(scoreModel.UID);
            scoreModel.Username = username;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(StatsToJSON(scoreModel)));
            Console.WriteLine("Sent User Stats");
        }
    }
}
