namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendQuestRequirement : QuestRequirement
    {
        public int RequiredOnlinedFriendNumber { get; private set; }

        public override string Description
        {
            get
            {
                return $"發送訊息給{RequiredOnlinedFriendNumber}位不同的好友";
            }
        }

        public SendMessageToDifferentOnlineFriendQuestRequirement(int requiredOnlinedFriendNumber)
        {
            RequiredOnlinedFriendNumber = requiredOnlinedFriendNumber;
        }
    }
}
