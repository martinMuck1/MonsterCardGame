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
    public class PutUserData : Handler
    {
        struct formatUserData
        {
            public string name;
            public string bio;
            public string image;
        }

        private formatUserData _reqUserData;

        public PutUserData( AuthLevel level) :base(level)
        {
            
        }

        public override void DeserializeMessage(string message)
        {
            _reqUserData = JsonConvert.DeserializeObject<formatUserData>(message);

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

            if (username != this.Param)
            {
                Console.WriteLine("User tried to access other users data");
                res.SendResponse(responseType.UNAUTHORIZED, "\"message\": \"you are not allowed to access this area\"");
                return;
            }

            UserDao userdao = new UserDao();
            UserModel modelUser = new UserModel(username, _reqUserData.name, _reqUserData.bio, _reqUserData.image);
            if (userdao.UpdateUserData(modelUser) != 0)
            {
                res.SendResponse(responseType.ERR, "{\"message\": \"could not update User Data\"}");
                return;
            }
            Console.WriteLine("updated User Data");
            res.SendResponse(responseType.OK, "{\"message\": \"updated user data successfully\"}");

        }
    }
}
