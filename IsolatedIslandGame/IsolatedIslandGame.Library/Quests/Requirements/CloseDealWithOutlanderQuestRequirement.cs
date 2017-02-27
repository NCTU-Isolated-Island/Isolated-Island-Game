using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealWithOutlanderQuestRequirement : QuestRequirement
    {
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealWithOutlander;
            }
        }
        public override string Description
        {
            get
            {
                return $"與陌生人交易成功";
            }
        }

        public CloseDealWithOutlanderQuestRequirement(int questRequirementID) : base(questRequirementID)
        {
        }
    }
}
