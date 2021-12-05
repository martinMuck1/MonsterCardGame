using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MonsterCardGame.Server
{
    abstract class  Handler : IHandler
    {

        public Handler()
        {
          
        }

        public abstract void Handle();
    }
}
