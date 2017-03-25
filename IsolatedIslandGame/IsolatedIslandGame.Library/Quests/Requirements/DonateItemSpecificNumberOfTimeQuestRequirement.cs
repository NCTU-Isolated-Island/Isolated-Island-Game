using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class DonateItemSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificNumberOfTime { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.DonateItemSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"送禮{SpecificNumberOfTime}次";
            }
        }

        public DonateItemSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificNumberOfTime) : base(questRequirementID)
        {
            SpecificNumberOfTime = specificNumberOfTime;
        }
    }
}
