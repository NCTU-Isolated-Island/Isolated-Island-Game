using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SynthesizeSpecificBlueprintQuestRequirement : QuestRequirement
    {
        public int SpecificBlueprintID { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SynthesizeSpecificBlueprint;
            }
        }
        public override string Description
        {
            get
            {
                Blueprint blueprint;
                if(BlueprintManager.Instance.FindBlueprint(SpecificBlueprintID, out blueprint))
                {
                    return $"使用藍圖: {blueprint.ToString()}";
                }
                else
                {
                    return $"使用未知的藍圖";
                }
            }
        }

        public SynthesizeSpecificBlueprintQuestRequirement(int questRequirementID, int specificBlueprintID) : base(questRequirementID)
        {
            SpecificBlueprintID = specificBlueprintID;
        }
    }
}
