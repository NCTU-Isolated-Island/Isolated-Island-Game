using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class ExistedInSpecificNumberOceanQuestRequirement : QuestRequirement
    {
        public int SpecificOceanNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.ExistedInSpecificNumberOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"到過{SpecificOceanNumber}個不同的海域";
            }
        }

        public ExistedInSpecificNumberOceanQuestRequirement(int questRequirementID, int specificOceanNumber) : base(questRequirementID)
        {
            SpecificOceanNumber = specificOceanNumber;
        }
    }
}
