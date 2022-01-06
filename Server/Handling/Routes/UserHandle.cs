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
    public class UserHandle :Handler
    {
        struct formatUser
        {
            public string userName;
            public string password;
        }

        private formatUser _reqUser;

        public UserHandle( AuthLevel level) :base(level)
        {
        }

        public override void DeserializeMessage(string message)
        {
            _reqUser = JsonConvert.DeserializeObject<formatUser>(message);
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            IUserDao userdao = new UserDao();
            IDeckDao deckdao = new DeckDao();
            IScoreDao scoredao = new ScoreDao();
            UserModel user;
            if (userdao.CreateUser((user = new UserModel(_reqUser.userName, Session.ComputeSha256Hash(_reqUser.password)))) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"User already exists!\"}");
                return;
            }
            user.SetUID();
            if (deckdao.CreateDeck(user) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"UserDeck already exists!\"}");
                return;
            }
            if (scoredao.CreateScoreStats(user) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"Score Stats already exists!\"}");
                return;
            }
            Console.WriteLine("Created User successfully");
            JObject obj = new JObject();
            obj["message"] = "created user successfully";
            obj["token"] = _reqUser.userName+ "-mtcgToken";
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
