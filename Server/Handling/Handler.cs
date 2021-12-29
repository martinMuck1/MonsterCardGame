using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonsterCardGame.Database;


namespace MonsterCardGame.Server
{
    public abstract class  Handler : IHandler
    {
        protected AuthLevel _authLevel;

        public Handler(AuthLevel lev)
        {
            this._authLevel = lev;
        }

        public bool CheckAuth(Response res, string token)
        {
            if (_authLevel == AuthLevel.noLogin)
                return true;
            if(_authLevel == AuthLevel.Login)
            {
                if (!Session.SessionDic.ContainsKey(token))
                {
                    Console.WriteLine("No login fullfilled => not authorized to succeed with this request");
                    res.SendResponse(responseType.UNAUTHORIZED, "{message: access denied}");
                    return false;
                }
                //request token is in memory => permission accepted
                Console.WriteLine("Token accepted");
                return true;
            }
            if(_authLevel == AuthLevel.Admin)
            {
                if (!Session.SessionDic.ContainsKey(token))
                {
                    Console.WriteLine("No login fullfilled => not authorized to succeed with this request");
                    res.SendResponse(responseType.UNAUTHORIZED, "{message: access denied}");
                    return false;
                }
                
                if (token == "admin-mtcgToken")     //normally this should be in db
                {
                    Console.WriteLine("Admin Token accepted");
                    return true;
                }
            }
            res.SendResponse(responseType.UNAUTHORIZED, "{message: access denied}");
            return false;
        }

        public abstract void Handle(Response res, string token);
        public abstract void DeserializeMessage(string message);
    }
}
