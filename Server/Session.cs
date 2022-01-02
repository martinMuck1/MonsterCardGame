using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Database;

namespace MonsterCardGame.Server
{
    public struct sessionData
    {
        string username;
        string token;
    }
    public class Session
    {
        //registers only logged in users in dictionary
        public static Dictionary<string, string> SessionDic { get; } = new Dictionary<string, string>();
        public static Dictionary<int, string> UserDic { get; } = new Dictionary<int, string>();

        static readonly object _lockObject = new object();
        public static void AddEntry(string token, string username) {
            //string tmpGuid =  Guid.NewGuid().ToString();
            SessionDic.Add(token, username);
        }
        public static void SetUserDic()
        {
            UserDic.Clear();
            UserDao userDao = new UserDao();
            List<UserModel> modelList = userDao.GetAllUsers();
            modelList.ForEach((item) => UserDic.Add(item.UID, item.Username));
        }
    }
}
