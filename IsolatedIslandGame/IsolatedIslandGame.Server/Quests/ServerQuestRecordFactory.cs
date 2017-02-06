using System;
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

        public override bool CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(int questRecordID, int playerID, QuestRequirement requirement, out QuestRequirementRecord record)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.CreateSendMessageToDifferentOnlineFriendTheSameOceanQuestRequirementRecord(questRecordID, requirement, out record);
        }

        public override bool AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord(int requirementRecordID, int onlineFriendPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToDifferentOnlineFriendQuestRequirementRecord(requirementRecordID, onlineFriendPlayerID);
        }
    }
}
