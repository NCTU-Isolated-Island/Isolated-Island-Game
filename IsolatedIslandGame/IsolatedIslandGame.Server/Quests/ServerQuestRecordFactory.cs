using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;

namespace IsolatedIslandGame.Server.Quests
{
    public class ServerQuestRecordFactory : QuestRecordFactory
    {
        public override bool CreateQuestRecord(int playerID, Quest quest, out QuestRecord record)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.CreateQuestRecord(playerID, quest, out record);
        }
        public override bool CreateQuestRequirementRecord(int questRecordID, int playerID, QuestRequirement requirement, out QuestRequirementRecord record)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.CreateQuestRequirementRecord(questRecordID, requirement, out record);
        }
        public override bool MarkMarkQuestRecordHasGottenReward(int questRecordID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.MarkQuestRecordHasGottenReward(questRecordID);
        }

        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(requirementRecordID, friendPlayerID);
        }
        public override bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(requirementRecordID, friendPlayerID);
        }
    }
}
