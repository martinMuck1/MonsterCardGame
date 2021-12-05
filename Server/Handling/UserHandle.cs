using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MonsterCardGame.Server
{
    class UserHandle :Handler
    {
        struct formatUser
        {
            public string userName;
            public string password;
        }

        formatUser _reqUser;

        public UserHandle(string message):base()
        {
            _reqUser = JsonConvert.DeserializeObject<formatUser>(message);
        }

        public override void Handle()
        {
            Console.WriteLine(_reqUser.userName);
            Console.WriteLine(_reqUser.password);
        }
    }
}
