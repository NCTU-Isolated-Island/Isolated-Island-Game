namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendQuestRequirement : QuestRequirement
    {
        public int FriendCount { get; private set; }

        public override string Description
        {
            get
            {
                return $"發送訊息給{FriendCount}位不同的好友";
            }
        }

        public SendMessageToDifferentOnlineFriendQuestRequirement(int friendCount)
        {
            FriendCount = friendCount;
        }
    }
}
