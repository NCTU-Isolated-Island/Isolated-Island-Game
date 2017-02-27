using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class AddSpecificNumberDecorationToVesselQuestRequirement : QuestRequirement
    {
        public int SpecificDecorationNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.AddSpecificNumberDecorationToVessel;
            }
        }
        public override string Description
        {
            get
            {
                return $"擺放裝飾到船上{SpecificDecorationNumber}次";
            }
        }

        public AddSpecificNumberDecorationToVesselQuestRequirement(int questRequirementID, int specificDecorationNumber) : base(questRequirementID)
        {
            SpecificDecorationNumber = specificDecorationNumber;
        }
    }
}
