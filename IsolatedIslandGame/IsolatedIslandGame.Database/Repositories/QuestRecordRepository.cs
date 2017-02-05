using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class QuestRecordRepository
    {
        public abstract bool CreateQuestRecord(Player player, Quest quest, out QuestRecord questRecord);
        public abstract List<QuestRecord> ListOfPlayer(Player player);

        protected abstract List<QuestRequirementRecord> ListRequirementRecordsOfQuestRecord(QuestRecord questRecord, Player player);

        public abstract bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeRequirementRecordToSendMessageToDifferentOnlineFriendRequirementRecord(int requirementRecordID, QuestRequirement requirement, Player player, out QuestRequirementRecord requirementRecord);
    }
}
