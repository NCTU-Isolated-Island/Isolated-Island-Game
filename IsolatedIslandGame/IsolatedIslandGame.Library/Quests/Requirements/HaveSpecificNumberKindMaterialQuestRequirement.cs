using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class HaveSpecificNumberKindMaterialQuestRequirement : QuestRequirement
    {
        public int SpecificKindNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.HaveSpecificNumberKindMaterial;
            }
        }
        public override string Description
        {
            get
            {
                return $"擁有{SpecificKindNumber}種素材";
            }
        }

        public HaveSpecificNumberKindMaterialQuestRequirement(int questRequirementID, int specificKindNumber) : base(questRequirementID)
        {
            SpecificKindNumber = specificKindNumber;
        }
    }
}
