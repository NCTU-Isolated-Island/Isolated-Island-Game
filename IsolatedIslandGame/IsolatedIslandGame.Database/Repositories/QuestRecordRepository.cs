using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Library.Quests.RequirementRecords;
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
        public abstract bool IsPlayerHasAnyStillNotGottenRewardQuest(int playerID, int questID);

        #region create quest requirement record
        protected abstract bool CreateCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSynthesizeSpecificBlueprintQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateFinishedBeforeSpecificTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateFinishedInSpecificTimeSpanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateDrawMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateDonateItemSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateHaveSpecificNumberSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool CreateStillNotOpenedQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        #endregion

        #region specialize quest requirement record
        protected abstract bool SpecializeQuestRequirementRecordToCumulativeLoginSpecificDayQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToStayInSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToCloseDealSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSendMaterialToIslandSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToExistedInSpecificNumberOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToGetSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToCloseDealWithOutlanderQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToCollectSpecificNumberBelongingGroupMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSynthesizeSpecificBlueprintQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToScanSpecificQR_CodeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToHaveSpecificNumberFriendQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToHaveSpecificNumberKindMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToAddSpecificNumberDecorationToVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToHaveSpecificNumberDecorationOnVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToFinishedBeforeSpecificTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToFinishedInSpecificTimeSpanQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToDrawMaterialQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSendMessageToSpecificNumberDifferentPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToDonateItemSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToRemoveSpecificNumberDecorationFromVesselQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToHaveSpecificNumberSpecificItemQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        protected abstract bool SpecializeQuestRequirementRecordToStillNotOpenedQuestRequirementRecord(int requirementRecordID, QuestRequirement requirement, out QuestRequirementRecord requirementRecord);
        #endregion

        #region update quest requirement record
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
        #endregion
    }
}
