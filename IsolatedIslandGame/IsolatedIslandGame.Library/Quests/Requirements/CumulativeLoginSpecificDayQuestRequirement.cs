using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CumulativeLoginSpecificDayQuestRequirement : QuestRequirement
    {
        public int SpecificDayCount { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CumulativeLoginSpecificDay;
            }
        }
        public override string Description
        {
            get
            {
                return $"累計登入{SpecificDayCount}天";
            }
        }

        public CumulativeLoginSpecificDayQuestRequirement(int questRequirementID, int specificDayCount) : base(questRequirementID)
        {
            SpecificDayCount = specificDayCount;
        }
    }
}
