using IsolatedIslandGame.Database.Repositories;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace IsolatedIslandGame.Database.MySQL.Repositories
{
    class MySQL_QuestRecordRepository : QuestRecordRepository
    {
        public override List<QuestRecord> ListOfPlayer(int playerID)
        {
            throw new NotImplementedException();
        }
    }
}
