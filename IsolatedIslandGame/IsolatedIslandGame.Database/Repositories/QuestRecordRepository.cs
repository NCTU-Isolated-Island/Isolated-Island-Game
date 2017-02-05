using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class QuestRecordRepository
    {
        public abstract List<QuestRecord> ListOfPlayer(int playerID);
    }
}
