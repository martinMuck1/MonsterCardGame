using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MonsterCardGame.Server
{
    public abstract class  Handler : IHandler
    {
        protected authLevel _authLevel;

        public Handler(authLevel lev)
        {
            this._authLevel = lev;
        }

        public bool CheckAuth(Response res, string token)
        {
            if (_authLevel == authLevel.noLogin)
                return true;
            if(_authLevel == authLevel.Login)
            {
                //TODO check if token exists
                //if not => send 401
            }
            if(_authLevel == authLevel.Admin)
            {
                //TODO check if this is admin token  
            }
            return false;
        }

        public abstract void Handle(Response res, string token);
    }
}
