using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement : QuestRequirement
    {
        public OceanType SpecificOceanType { get; private set; }
        public int SpecificFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"在{OceanNameMapping.GetOceanName(SpecificOceanType)}海域中，與 {SpecificFriendNumber}位 在同海域中的好友完成交易";
            }
        }

        public CloseDealWithSpecificNumberDifferentFriendInTheSameSpecificOceanQuestRequirement(int questRequirementID, OceanType specificOceanType, int specificFriendNumber) : base(questRequirementID)
        {
            SpecificOceanType = specificOceanType;
            SpecificFriendNumber = specificFriendNumber;
        }
    }
}
