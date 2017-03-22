using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class DrawMaterialQuestRequirement : QuestRequirement
    {
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.DrawMaterial;
            }
        }
        public override string Description
        {
            get
            {
                return $"抽取素材";
            }
        }

        public DrawMaterialQuestRequirement(int questRequirementID) : base(questRequirementID)
        {
        }
    }
}
