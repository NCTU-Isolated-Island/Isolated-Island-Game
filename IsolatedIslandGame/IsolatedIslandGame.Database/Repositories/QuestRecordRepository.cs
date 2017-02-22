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

        public abstract bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
        protected abstract bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendTheSameSpecificOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);

        public abstract bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID);
        protected abstract bool SpecializeRequirementRecordToCloseDealWithDifferentFriendInTheSameSpecificOceanRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);

        public abstract bool MarkScanQR_CodeQuestRequirementRecordHasScannedCorrectQR_Code(int requirementRecordID);
        protected abstract bool SpecializeRequirementRecordToScanQR_CodeRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);

        protected abstract bool SpecializeRequirementRecordToCumulativeLoginRequirementRecord(int requirementRecordID, QuestRequirement requirement, int playerID, out QuestRequirementRecord requirementRecord);
    }
}
