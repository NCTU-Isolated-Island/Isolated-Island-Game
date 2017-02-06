using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class QuestRecordRepository
    {
        protected struct QuestRecordInfo
        {
            public int questRecordID;
            public int questID;
        }
        protected struct QuestRequirementRecordInfo
        {
            public int questRequirementRecordID;
            public int questRequirementID;
            public QuestRequirementType questRequirementType;
        }
        public abstract bool CreateQuestRecord(Player player, Quest quest, out QuestRecord questRecord);
        public abstract List<QuestRecord> ListOfPlayer(Player player);

        protected abstract List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(int questRecordID, Player player);

        public abstract bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(int questRecordID, Player player, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        public abstract bool AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord(int requirementRecordID, int onlineFriendPlayerID);
        protected abstract bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendRequirementRecord(int requirementRecordID, QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord);
    }
}
