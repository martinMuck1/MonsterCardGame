using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface IUserDao
    {
        public int CreateUser(UserModel user);
        public int LoginUser(UserModel user);
        public int PayWithCoins(string username, int amount);
        public int CheckAccountBalance(string username);
        public int GetUserID(string username);
        public UserModel GetUserData(string username);
        public int UpdateUserData(UserModel user);
        public List<UserModel> GetAllUsers();
    }
}
