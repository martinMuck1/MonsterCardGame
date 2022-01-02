using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardGame.Database
{
    public interface IScoreDao
    {
        public int CreateScoreStats(UserModel user);
        public ScoreModel ShowUserStats(int uid);
        public List<ScoreModel> ShowScoreBoard();
        public BattleModel ShowRequests();
        public int CreateBattleRequest(BattleModel model);
        public int UpdateScoreBoard(ScoreModel model);
        public int DeleteBattleRequest(BattleModel model);
    }
}
