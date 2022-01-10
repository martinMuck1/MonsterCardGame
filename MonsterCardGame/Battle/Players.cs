using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Battle
{
    public class Players
    {
        public string Name { get; private set; }
        public AbstractDeckManager MyDeck { get; set; } 
        
        public Players(string name)    //create user with random deck
        {
            this.Name = name;
        }
    }
}
