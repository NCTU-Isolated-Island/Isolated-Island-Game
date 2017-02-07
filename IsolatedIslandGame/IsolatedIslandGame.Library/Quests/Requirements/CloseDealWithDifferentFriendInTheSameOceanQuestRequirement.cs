using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealWithDifferentFriendInTheSameOceanQuestRequirement : QuestRequirement
    {
        [MessagePackMember(1)]
        public int RequiredFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealWithDifferentFriendInTheSameOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"與 {RequiredFriendNumber}位 好友在同海域中完成交易";
            }
        }

        [MessagePackDeserializationConstructor]
        public CloseDealWithDifferentFriendInTheSameOceanQuestRequirement() { }
        public CloseDealWithDifferentFriendInTheSameOceanQuestRequirement(int questRequirementID, int requiredFriendNumber) : base(questRequirementID)
        {
            RequiredFriendNumber = requiredFriendNumber;
        }
    }
}
