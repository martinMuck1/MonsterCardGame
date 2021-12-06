using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MonsterCardGame.Database;

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

        public UserHandle(string message):base()
        {
            _reqUser = JsonConvert.DeserializeObject<formatUser>(message);
        }

        public override responseType Handle()
        {
            IUserDao userdao = new UserDao();
            userdao.createUser(new UserModel(_reqUser.userName, _reqUser.password));

            return responseType.OK;
        }
    }
}
