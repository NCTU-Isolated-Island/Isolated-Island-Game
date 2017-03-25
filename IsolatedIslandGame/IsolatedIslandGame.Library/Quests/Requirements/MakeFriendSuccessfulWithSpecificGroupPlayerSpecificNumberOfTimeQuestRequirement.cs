using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public GroupType SpecificGroupType { get; private set; }
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"結交{SpecificNumberOfTime}位{GroupNameMapping.GetGroupName(SpecificGroupType)}陣營的朋友";
            }
        }

        public MakeFriendSuccessfulWithSpecificGroupPlayerSpecificNumberOfTimeQuestRequirement(int questRequirementID, GroupType specificGroupType, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificGroupType = specificGroupType;
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
