﻿using IsolatedIslandGame.Library;
using IsolatedIslandGame.Library.Quests;
using IsolatedIslandGame.Protocol;
using System.Collections.Generic;

namespace IsolatedIslandGame.Database.Repositories
{
    public abstract class QuestRepository
    {
        protected struct QuestInfo
        {
            public int questID;
            public QuestType questType;
            public string questName;
            public string questDescription;
            public bool isHidden;
        }
        protected struct QuestRequirementInfo
        {
            public int questRequirementID;
            public QuestRequirementType questRequirementType;
        }
        protected struct QuestRewardInfo
        {
            public int questRewardID;
            public QuestRewardType questRewardType;
        }

        public abstract List<Quest> ListAll();

        protected abstract List<QuestRequirement> ListRequirementsOfQuest(int questID);

        protected abstract List<QuestReward> ListRewardsOfQuest(int questID);
        public abstract List<int> ListQuestIDsWhenRegistered();
        public abstract Dictionary<GroupType, List<int>> ListQuestIDsWhenChosedGroup();
        public abstract List<int> ListQuestIDsWhenTodayFirstLogin();
        public abstract List<int> ListQuestIDsWhenEveryHourPassed();
        public abstract List<int> ListQuestIDsWhenEveryDayPassed();
        public abstract Dictionary<OceanType, List<int>> ListQuestIDsWhenEnteredSpecificOcean();

        #region Specialize QuestRequirement
        protected abstract bool SpecializeQuestRequirementToCumulativeLoginSpecificDayQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToStayInSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToCloseDealSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSendMaterialToIslandSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToExistedInSpecificNumberOceanQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToCloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToGetSpecificItemQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToCloseDealWithOutlanderQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToCollectSpecificNumberBelongingGroupMaterialQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSynthesizeSpecificBlueprintQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToScanSpecificQR_CodeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToHaveSpecificNumberFriendQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToHaveSpecificNumberKindMaterialQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToAddSpecificNumberDecorationToVesselQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToHaveSpecificNumberDecorationOnVesselQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToFinishedBeforeSpecificTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToFinishedInSpecificTimeSpanQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToDrawMaterialQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSendMessageToSpecificNumberDifferentPlayerQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToDonateItemSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToRemoveSpecificNumberDecorationFromVesselQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToHaveSpecificNumberSpecificItemQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToMakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToMakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToSendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeQuestRequirementToStillNotOpenedQuestRequirement(int requirementID, out QuestRequirement requirement);
        #endregion

        #region Specialize QuestReward
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificItemQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToUnlockSpecificBlueprintQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToAcceptSpecificQuestQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreSpecificGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificScoreBelongingGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificLevelRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificLevelSpecificGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificLevelBelongingGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToGiveSpecificNumberSpecificLevelOtherGroupRandomMaterialQuestReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeQuestRewardToRemoveSpecificNumberSpecificItemQuestReward(int rewardID, out QuestReward reward);
        #endregion
    }
}
