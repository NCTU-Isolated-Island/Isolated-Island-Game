using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"完成{SpecificNumberOfTime}次交易";
            }
        }

        public CloseDealSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
