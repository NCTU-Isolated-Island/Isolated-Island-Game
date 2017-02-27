using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement : QuestRequirement
    {
        public int SpecificSuccessfulNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SynthesizeSuccessfulSpecificNumberOfTime;
            }
        }
        public override string Description
        {
            get
            {
                return $"合成成功{SpecificSuccessfulNumber}次";
            }
        }

        public SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement(int questRequirementID, int specificSuccessfulNumber) : base(questRequirementID)
        {
            SpecificSuccessfulNumber = specificSuccessfulNumber;
        }
    }
}
