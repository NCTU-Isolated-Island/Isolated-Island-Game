using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class IslandMaterialRepository
    {
        public abstract int ReadTotalScore(GroupType groupType);
        public abstract List<Island.PlayerMaterialInfo> ListTodayMaterialRanking();
        public abstract List<Island.PlayerScoreInfo> ListPlayerScoreRanking();
        public abstract void Save(Island.PlayerMaterialInfo info);
    }
}
