using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_IslandMaterialRepository : IslandMaterialRepository
    {
        public override int ReadTotalScore(GroupType groupType)
        {
            throw new NotImplementedException();
        }
        public override List<Island.PlayerMaterialInfo> ListTodayMaterialRanking()
        {
            throw new NotImplementedException();
        }
        public override List<Island.PlayerScoreInfo> ListPlayerScoreRanking()
        {
            throw new NotImplementedException();
        }

        public override void Save(Island.PlayerMaterialInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
