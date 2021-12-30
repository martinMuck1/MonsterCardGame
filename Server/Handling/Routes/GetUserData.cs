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
    public class GetUserData :Handler
    {
        public GetUserData( AuthLevel level) :base(level)
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
            
            if(username != this.Param)
            {
                Console.WriteLine("User tried to access other users data");
                res.SendResponse(responseType.UNAUTHORIZED, "\"message\": \"you are not allowed to access this area\"");
                return;
            }

            UserDao userdao = new UserDao();
            UserModel modelUser = userdao.GetUserData(username);
            JObject obj = new JObject();
            obj["username"] = modelUser.Username;
            obj["name"] = modelUser.Name;
            obj["bio"] = modelUser.Bio;
            obj["image"] = modelUser.Image;
            res.SendResponse(responseType.OK, JsonConvert.SerializeObject(obj));
            Console.WriteLine("Sent User Data");
        }
    }
}
