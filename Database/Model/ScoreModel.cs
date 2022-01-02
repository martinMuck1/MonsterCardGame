using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public class ScoreModel

    {
        public string Username { get; private set; }
        public int UID { get; set; }
        public int Elo { get; private set; }
        public int Wins { get; private set; }
        public int Loses { get; private set; }
        public int Games { get; private  set; }

        public ScoreModel(string username)
        {
            this.Username = username;
            SetUID();
        }
        public void ScoreModelOffset(int elo, int wins, int games, int loses)
        {
            this.Elo = elo;
            this.Wins = wins;
            this.Games = games;
            this.Loses = loses;
        }
        public ScoreModel(int uid,int elo, int wins, int games, int loses)
        {
            this.UID = uid;
            this.Elo = elo;
            this.Wins = wins;
            this.Games = games;
            this.Loses = loses;
        }

        public void SetUID()
        {
            this.UID = DBHelper.ConvertNameToID(this.Username);
        }


    }
}
