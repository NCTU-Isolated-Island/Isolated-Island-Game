using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CollectSpecificNumberBelongingGroupMaterialQuestRequirement : QuestRequirement
    {
        public int SpecificMaterialNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CollectSpecificNumberBelongingGroupMaterial;
            }
        }
        public override string Description
        {
            get
            {
                return $"蒐集{SpecificMaterialNumber}個所屬陣營的素材";
            }
        }

        public CollectSpecificNumberBelongingGroupMaterialQuestRequirement(int questRequirementID, int specificMaterialNumber) : base(questRequirementID)
        {
            SpecificMaterialNumber = specificMaterialNumber;
        }
    }
}
