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

        public abstract List<Quest> ListAll();

        protected abstract List<QuestRequirement> ListRequirementsOfQuest(int questID);

        protected abstract List<QuestReward> ListRewardsOfQuest(int questID);

        #region Specialize QuestRequirement
        protected abstract bool SpecializeRequirementToSendMessageToDifferentOnlineFriendTheSameOceanRequirement(int requirementID, out QuestRequirement requirement);
        #endregion

        #region Specialize QuestReward
        protected abstract bool SpecializeRewardToGiveItemReward(int rewardID, out QuestReward reward);
        #endregion
    }
}
