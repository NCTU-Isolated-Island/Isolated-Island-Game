using System;

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

        public SendMessageToDifferentOnlineFriendQuestRequirement(int questRequirementID, int requiredOnlinedFriendNumber) : base(questRequirementID)
        {
            RequiredOnlinedFriendNumber = requiredOnlinedFriendNumber;
        }

        public override QuestRequirementRecord CreateRequirementRecord(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
