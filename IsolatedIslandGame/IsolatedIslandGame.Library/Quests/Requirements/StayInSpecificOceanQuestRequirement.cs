using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class StayInSpecificOceanQuestRequirement : QuestRequirement
    {
        public OceanType SpecificOceanType { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.StayInSpecificOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"存在於{OceanNameMapping.GetOceanName(SpecificOceanType)}海域中";
            }
        }

        public StayInSpecificOceanQuestRequirement(int questRequirementID, OceanType specificOcean) : base(questRequirementID)
        {
            SpecificOceanType = specificOcean;
        }
    }
}
