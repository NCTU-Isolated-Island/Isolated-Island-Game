using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.MakeFriendSuccessfulSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"結交{SpecificNumberOfTime}位朋友";
            }
        }

        public MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
