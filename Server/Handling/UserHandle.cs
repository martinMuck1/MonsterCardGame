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

        public UserHandle(string message, authLevel level) :base(level)
        {
            _reqUser = JsonConvert.DeserializeObject<formatUser>(message);
        }

        public override void Handle(Response res,string token)
        {
            if (!CheckAuth(res, token))
                return;

            IUserDao userdao = new UserDao();
            //if (userdao.createUser(new UserModel(_reqUser.userName, _reqUser.password)) != 0)
            //    res.SendBadRequest(responseType.ERR);
            JObject obj = new JObject();
            obj["message"] = "created user successfully";
            obj["token"] = "sometoken";
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
        }
    }
}
