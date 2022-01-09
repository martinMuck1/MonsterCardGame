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
    public class GetScoreboard :Handler
    {
        private Dictionary<int, string> _user = new Dictionary<int, string>();
        public GetScoreboard( AuthLevel level) :base(level)
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
            List<ScoreModel> scoreModel =scoredao.ShowScoreBoard();
            getUserList();
            JArray array = new JArray();
            foreach (var singleScore in scoreModel)
            {
                if (!_user.ContainsKey(singleScore.UID))
                {
                    Console.WriteLine("couldnt get username from dictionary for scoreboard");
                    continue;
                }
                singleScore.Username = _user[singleScore.UID];
                array.Add(StatsToJSON(singleScore));
            }

            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(array));
            Console.WriteLine("Sent scoreboard");
        }

        private void getUserList()
        {
            UserDao userDao = new UserDao();
            List<UserModel> modelList = userDao.GetAllUsers();
            modelList.ForEach((item) => _user.Add(item.UID, item.Username));
        }
    }
}
