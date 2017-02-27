using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class HaveSpecificNumberDecorationOnVesselQuestRequirement : QuestRequirement
    {
        public int SpecificDecorationNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.HaveSpecificNumberDecorationOnVessel;
            }
        }
        public override string Description
        {
            get
            {
                return $"在船上擺放{SpecificDecorationNumber}個裝飾";
            }
        }

        public HaveSpecificNumberDecorationOnVesselQuestRequirement(int questRequirementID, int specificDecorationNumber) : base(questRequirementID)
        {
            SpecificDecorationNumber = specificDecorationNumber;
        }
    }
}
