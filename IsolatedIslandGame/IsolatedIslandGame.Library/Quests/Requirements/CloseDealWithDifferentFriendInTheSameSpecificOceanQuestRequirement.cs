using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement : QuestRequirement
    {
        [MessagePackMember(1)]
        public OceanType SpecificOceanType { get; private set; }
        [MessagePackMember(2)]
        public int RequiredFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealWithDifferentFriendInTheSameSpecificOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"在{OceanNameMapping.GetOceanName(SpecificOceanType)}海域中，與 {RequiredFriendNumber}位 好友在同海域中完成交易";
            }
        }

        [MessagePackDeserializationConstructor]
        public CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement() { }
        public CloseDealWithDifferentFriendInTheSameSpecificOceanQuestRequirement(int questRequirementID, OceanType specificOceanType, int requiredFriendNumber) : base(questRequirementID)
        {
            SpecificOceanType = specificOceanType;
            RequiredFriendNumber = requiredFriendNumber;
        }
    }
}
