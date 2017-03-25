using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class RemoveSpecificNumberDecorationFromVesselQuestRequirement : QuestRequirement
    {
        public int SpecificDecorationNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.RemoveSpecificNumberDecorationFromVessel;
            }
        }
        public override string Description
        {
            get
            {
                return $"從船上移除{SpecificDecorationNumber}個裝飾";
            }
        }

        public RemoveSpecificNumberDecorationFromVesselQuestRequirement(int questRequirementID, int specificDecorationNumber) : base(questRequirementID)
        {
            SpecificDecorationNumber = specificDecorationNumber;
        }
    }
}
