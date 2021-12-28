using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class DBHelper
    {
        //Helper function to convert username to uid for further db operations => multiple usage across subclasses
        public static int ConvertNameToID(string username)
        {
            IUserDao userdao = new UserDao();
            UserModel model = new UserModel(username);
            return userdao.GetUserID(model.Username);
        }
    }
}
