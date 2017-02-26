using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CumulativeLoginQuestRequirement : QuestRequirement
    {
        public int CumulativeLoginCount { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CumulativeLogin;
            }
        }
        public override string Description
        {
            get
            {
                return $"累計登入{CumulativeLoginCount}天";
            }
        }

        public CumulativeLoginQuestRequirement(int questRequirementID, int cumulativeLoginCount) : base(questRequirementID)
        {
            CumulativeLoginCount = cumulativeLoginCount;
        }
    }
}
