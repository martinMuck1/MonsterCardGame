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
    public class LoginHandle :Handler
    {
        struct formatUser
        {
            public string userName;
            public string password;
        }

        private formatUser _reqUser;

        public LoginHandle( AuthLevel level) :base(level)
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
            int status;
            if ((status = userdao.LoginUser(new UserModel(_reqUser.userName, _reqUser.password))) != 0)
            {
                if(status == -1)
                    res.SendResponse(responseType.UNAUTHORIZED, "{message: User does not exist or pw wrong}");
                if(status == -2)
                    res.SendResponse(responseType.ERR, "{message: Query got rejected from DB}");
                return;
            }

            JObject obj = new JObject();
            obj["message"] = "login was successfull";
            obj["token"] = _reqUser.userName+ "-mtcgToken";     //TODO token from DB taken
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
