using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    interface IHandler
    {
        public bool CheckAuth(Response res, string token);
        public abstract void Handle(Response res, string token);
        
    }
}
