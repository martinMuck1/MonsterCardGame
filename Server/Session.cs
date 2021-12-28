using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    class Session
    {
        private static Session instance = new Session();

        private Session()
        {

        }

        //Get the only object available
        public static Session getInstance()
        {
            return instance;
        }
    }
}
