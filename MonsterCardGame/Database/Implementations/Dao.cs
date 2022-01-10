using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace MonsterCardGame.Database
{
    public class Dao
    {
        protected Database _db;
        public Dao()
        {
            _db = Database.getInstance();
        }
    }
}
