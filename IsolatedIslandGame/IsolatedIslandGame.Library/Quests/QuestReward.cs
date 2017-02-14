using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestReward
    {
        [MessagePackMember(0)]
        public int QuestRewardID { get; private set; }
        public abstract QuestRewardType QuestRewardType { get; }
        public abstract string Description { get; }

        [MessagePackDeserializationConstructor]
        public QuestReward() { }
        protected QuestReward(int questRewardID)
        {
            QuestRewardID = questRewardID;
        }

        public abstract bool GiveRewardCheck(Player player);
        public abstract void GiveReward(Player player);
    }
}
