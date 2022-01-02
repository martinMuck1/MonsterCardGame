using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        static readonly object _lockObject = new object();
        public static void AddEntry(string token, string username) {
            //string tmpGuid =  Guid.NewGuid().ToString();
            SessionDic.Add(token, username);
        }
    }
}
