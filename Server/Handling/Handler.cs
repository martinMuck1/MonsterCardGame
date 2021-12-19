using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
                //TODO check if token exists
                //if not => send 401
            }
            if(_authLevel == AuthLevel.Admin)
            {
                if(token == "admin-mtcgToken")
                {
                    Console.WriteLine("Token accepted");
                    return true;
                }
                //TODO check if this is admin token  
            }
            return false;
        }

        public abstract void Handle(Response res, string token);
        public abstract void DeserializeMessage(string message);
    }
}
