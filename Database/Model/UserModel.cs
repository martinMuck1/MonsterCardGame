using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class UserModel
    {
        public String Username { get; private set; }
        public String Password { get; private set; }

        public UserModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
