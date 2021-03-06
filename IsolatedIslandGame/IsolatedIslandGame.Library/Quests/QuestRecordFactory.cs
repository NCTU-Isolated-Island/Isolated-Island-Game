﻿using IsolatedIslandGame.Library.Quests.RequirementRecords;
using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRecordFactory
    {
        public static QuestRecordFactory Instance { get; private set; }
        public static void Initial(QuestRecordFactory factory)
        {
            Instance = factory;
        }

        public abstract bool CreateQuestRecord(int playerID, Quest quest, out QuestRecord record);
        public abstract bool CreateQuestRequirementRecord(int questRecordID, int playerID, QuestRequirement requirement, out QuestRequirementRecord record);
        public abstract bool MarkQuestRecordHasGottenReward(int questRecordID);

        public abstract void UpdateCumulativeLoginSpecificDayQuestRequirementRecord(CumulativeLoginSpecificDayQuestRequirementRecord record);
        public abstract void UpdateStayInSpecificOceanQuestRequirementRecord(StayInSpecificOceanQuestRequirementRecord record);
        public abstract void UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(CloseDealSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(SendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract bool AddOceanToExistedInSpecificNumberOceanQuestRequirementRecord(ExistedInSpecificNumberOceanQuestRequirementRecord record, OceanType locatedOceanType);
        public abstract bool AddPlayerIDToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID);
        public abstract bool AddPlayerIDToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord record, int theOtherPlayerID);
        public abstract void UpdateGetSpecificItemQuestRequirementRecord(GetSpecificItemQuestRequirementRecord record);
        public abstract void UpdateCloseDealWithOutlanderQuestRequirementRecord(CloseDealWithOutlanderQuestRequirementRecord record);
        public abstract void UpdateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(CollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord record);
        public abstract void UpdateSynthesizeSpecificBlueprintQuestRequirementRecord(SynthesizeSpecificBlueprintQuestRequirementRecord record);
        public abstract void UpdateScanSpecificQR_CodeQuestRequirementRecord(ScanSpecificQR_CodeQuestRequirementRecord record);
        public abstract void UpdateHaveSpecificNumberFriendQuestRequirementRecord(HaveSpecificNumberFriendQuestRequirementRecord record);
        public abstract void UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateHaveSpecificNumberKindMaterialQuestRequirementRecord(HaveSpecificNumberKindMaterialQuestRequirementRecord record);
        public abstract void UpdateAddSpecificNumberDecorationToVesselQuestRequirementRecord(AddSpecificNumberDecorationToVesselQuestRequirementRecord record);
        public abstract void UpdateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(HaveSpecificNumberDecorationOnVesselQuestRequirementRecord record);
        public abstract void UpdateFinishedBeforeSpecificTimeQuestRequirementRecord(FinishedBeforeSpecificTimeQuestRequirementRecord record);
        public abstract void UpdateFinishedInSpecificTimeSpanQuestRequirementRecord(FinishedInSpecificTimeSpanQuestRequirementRecord record);
        public abstract void UpdateDrawMaterialQuestRequirementRecord(DrawMaterialQuestRequirementRecord record);
        public abstract bool AddPlayerIDToSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(SendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord record, int theOtherPlayerID);
        public abstract void UpdateDonateItemSpecificNumberOfTimeQuestRequirementRecord(DonateItemSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(RemoveSpecificNumberDecorationFromVesselQuestRequirementRecord record);
        public abstract void UpdateMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord record);
        public abstract void UpdateCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord record);
        public abstract bool AddPlayerIDToSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord(SendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord record, int theOtherPlayerID);
    }
}
