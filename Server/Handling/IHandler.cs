using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Server
{
    public interface IHandler
    {
        public bool CheckAuth(Response res, string token, ref string username);
        public abstract void Handle(Response res, string token);
        public void DeserializeMessage(string message);
        public string Param { get; set; }

    }
}
