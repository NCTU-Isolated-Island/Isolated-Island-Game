using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement : QuestRequirement
    {
        [MessagePackMember(1)]
        public int RequiredFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"發送訊息給同海域的 {RequiredFriendNumber}位 在線好友";
            }
        }

        [MessagePackDeserializationConstructor]
        public SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement() { }
        public SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement(int questRequirementID, int requiredFriendNumber) : base(questRequirementID)
        {
            RequiredFriendNumber = requiredFriendNumber;
        }
    }
}
