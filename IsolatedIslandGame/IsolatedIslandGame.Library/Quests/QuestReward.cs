using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestReward
    {
        public int QuestRewardID { get; private set; }
        public abstract QuestRewardType QuestRewardType { get; }
        public abstract string Description { get; }

        protected QuestReward(int questRewardID)
        {
            QuestRewardID = questRewardID;
        }

        public abstract bool GiveRewardCheck(Player player);
        public abstract void GiveReward(Player player);
    }
}
