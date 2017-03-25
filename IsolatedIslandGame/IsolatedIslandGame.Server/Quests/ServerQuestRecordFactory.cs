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
        public override void UpdateSynthesizeSpecificBlueprintQuestRequirementRecord(SynthesizeSpecificBlueprintQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateSynthesizeSpecificBlueprintQuestRequirementRecord(record);
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
        public override void UpdateDrawMaterialQuestRequirementRecord(DrawMaterialQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateDrawMaterialQuestRequirementRecord(record);
        }
        public override bool AddPlayerIDToSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(SendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord record, int theOtherPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(record, theOtherPlayerID);
        }
        public override void UpdateDonateItemSpecificNumberOfTimeQuestRequirementRecord(DonateItemSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateDonateItemSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(RemoveSpecificNumberDecorationFromVesselQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(record);
        }
        public override void UpdateMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(record);
        }
        public override void UpdateCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord record)
        {
            DatabaseService.RepositoryList.QuestRecordRepository.UpdateCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(record);
        }
        public override bool AddPlayerIDToSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord(SendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord record, int theOtherPlayerID)
        {
            return DatabaseService.RepositoryList.QuestRecordRepository.AddPlayerIDToSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord(record, theOtherPlayerID);
        }
    }
}
