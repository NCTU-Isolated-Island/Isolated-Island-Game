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
            public bool hasGottenReward;
        }
        protected struct QuestRequirementRecordInfo
        {
            public int questRequirementRecordID;
            public int questRequirementID;
            public QuestRequirementType questRequirementType;
        }
        public abstract bool CreateQuestRecord(int playerID, Quest quest, out QuestRecord questRecord);
        public abstract List<QuestRecord> ListOfPlayer(int playerID);
        protected abstract List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(int questRecordID, int playerID);
        public abstract bool CreateQuestRequirementRecord(int questRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        public abstract bool MarkQuestRecordHasGottenReward(int questRecordID);

        public abstract bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
        protected abstract bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendTheSameOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);

        public abstract bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
        protected abstract bool SpecializeRequirementRecordToCloseDealWithDifferentFriendInTheSameOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);
    }
}
