using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public static readonly object _lockObject = new object();           
        public static ConcurrentDictionary<string, string> SessionDic { get; set; } = new ConcurrentDictionary<string, string>();         //registers only logged in users in dictionary
        public static ConcurrentDictionary<int, string> UserDic { get; } = new ConcurrentDictionary<int, string>();

        public static void SetUserDic()
        {
            UserDic.Clear();
            UserDao userDao = new UserDao();
            List<UserModel> modelList = userDao.GetAllUsers();
            modelList.ForEach((item) => UserDic.TryAdd(item.UID, item.Username));
        }

        public static string ComputeSha256Hash(string data)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static double CalculateRatio(int wins, int loses, int games)
        {
            if (games == 0)
                return 0;
            int ties = games - wins - loses;
            double val = (((2 * wins) + ties) / (2 * (double)games));
            return val;
        }
    }
}
