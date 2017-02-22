using IsolatedIslandGame.Library;
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

        #region Specialize QuestRequirement
        protected abstract bool SpecializeRequirementToSendMessageToDifferentOnlineFriendTheSameSpecificOceanRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeRequirementToCloseDealWithDifferentFriendInTheSameSpecificOceanRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeRequirementToScanQR_CodeRequirement(int requirementID, out QuestRequirement requirement);
        protected abstract bool SpecializeRequirementToCumulativeLoginRequirement(int requirementID, out QuestRequirement requirement);
        #endregion

        #region Specialize QuestReward
        protected abstract bool SpecializeRewardToGiveItemReward(int rewardID, out QuestReward reward);
        protected abstract bool SpecializeRewardToUnlockBlueprintReward(int rewardID, out QuestReward reward);
        #endregion
    }
}
