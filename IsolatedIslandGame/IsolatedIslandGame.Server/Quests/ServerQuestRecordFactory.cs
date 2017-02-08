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
        public override bool MarkQuestRecordHasGottenReward(int questRecordID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.MarkQuestRecordHasGottenReward(questRecordID);
        }

        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, friendPlayerID);
        }
        public override bool AddPlayerIDToCloseDealWithDifferentFriendInTheSameOceanQuestRequirementRecord(int requirementRecordID, int friendPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToCloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(requirementRecordID, friendPlayerID);
        }
        public override bool MarkScanQR_CodeQuestRequirementRecordHasScannedCorrectQR_Code(int requirementRecordID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.MarkScanQR_CodeQuestRequirementRecordHasScannedCorrectQR_Code(requirementRecordID);
        }
    }
}
