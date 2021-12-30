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
        public int UID { get; set; }
        public string Name { get; private set; }
        public string Bio { get; private set; }
        public string Image { get; private set; }


        public UserModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.Token = username + "-mtcgToken";
        }

        public UserModel(string username)
        {
            this.Username = username;
        }
        public UserModel(string username,string name, string bio, string image)
        {
            this.Username = username;
            this.Name = name;
            this.Bio = bio;
            this.Image = image;
        }

        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }


    }
}
