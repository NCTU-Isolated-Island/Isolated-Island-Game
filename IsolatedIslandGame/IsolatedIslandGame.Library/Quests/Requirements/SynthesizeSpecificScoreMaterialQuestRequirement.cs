using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SynthesizeSpecificScoreMaterialQuestRequirement : QuestRequirement
    {
        public int SpecificMaterialScore { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SynthesizeSpecificScoreMaterial;
            }
        }
        public override string Description
        {
            get
            {
                return $"合成出{SpecificMaterialScore}分的素材";
            }
        }

        public SynthesizeSpecificScoreMaterialQuestRequirement(int questRequirementID, int specificMaterialScore) : base(questRequirementID)
        {
            SpecificMaterialScore = specificMaterialScore;
        }
    }
}
