using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class UserModel
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Token { get; private set; }

        public UserModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.Token = username + "-mtcgToken";
        }
    }
}
