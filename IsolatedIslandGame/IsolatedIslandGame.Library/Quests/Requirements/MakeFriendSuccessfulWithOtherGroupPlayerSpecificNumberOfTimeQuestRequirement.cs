using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"結交{SpecificNumberOfTime}位和自己不同陣營的朋友";
            }
        }

        public MakeFriendSuccessfulWithOtherGroupPlayerSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
