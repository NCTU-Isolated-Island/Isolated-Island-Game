using IsolatedIslandGame.Database;
using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.Quests.RequirementRecords;
using IsolatedIslandGame.Protocol;

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

        public override void UpdateCumulativeLoginSpecificDayQuestRequirementRecord(CumulativeLoginSpecificDayQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateCumulativeLoginSpecificDayQuestRequirementRecord(record);
        }
        public override void UpdateStayInSpecificOceanQuestRequirementRecord(StayInSpecificOceanQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateStayInSpecificOceanQuestRequirementRecord(record);
        }
        public override void UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(CloseDealSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override bool AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(ExistedInSpecificNumberOceanQuestRequirementRecord record, OceanType locatedOceanType)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(record, locatedOceanType);
        }
        public override bool AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(record, theOtherPlayerID);
        }
        public override bool AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(record, theOtherPlayerID);
        }
        public override void UpdateGetSpecificItemQuestRequirementRecord(GetSpecificItemQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateGetSpecificItemQuestRequirementRecord(record);
        }
        public override void UpdateCloseDealWithOutlanderQuestRequirementRecord(CloseDealWithOutlanderQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateCloseDealWithOutlanderQuestRequirementRecord(record);
        }
        public override void UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(record);
        }
        public override void UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord(SynthesizeSpecificScoreMaterialQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateSynthesizeSpecificScoreMaterialQuestRequirementRecord(record);
        }
        public override void UpdateScanSpecificQR_CodeQuestRequirementRecord(ScanSpecificQR_CodeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateScanSpecificQR_CodeQuestRequirementRecord(record);
        }
        public override void UpdateHaveSpecificNumberFriendQuestRequirementRecord(HaveSpecificNumberFriendQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateHaveSpecificNumberFriendQuestRequirementRecord(record);
        }
        public override void UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(HaveSpecificNumberKindMaterialQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(record);
        }
        public override void UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(AddSpecificNumberDecorationToVesselQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(record);
        }
        public override void UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(HaveSpecificNumberDecorationOnVesselQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(record);
        }
        public override void UpdateFinishedBeforeSpecificTimeQuestRequirementRecord(FinishedBeforeSpecificTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateFinishedBeforeSpecificTimeQuestRequirementRecord(record);
        }
        public override void UpdateFinishedInSpecificTimeSpanQuestRequirementRecord(FinishedInSpecificTimeSpanQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateFinishedInSpecificTimeSpanQuestRequirementRecord(record);
        }
    }
}
