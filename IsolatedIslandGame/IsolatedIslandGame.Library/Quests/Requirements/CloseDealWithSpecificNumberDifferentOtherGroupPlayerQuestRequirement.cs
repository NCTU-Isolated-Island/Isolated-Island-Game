using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirement : QuestRequirement
    {
        public int SpecificNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.CloseDealWithSpecificNumberDifferentOtherGroupPlayer;
            }
        }
        public override string Description
        {
            get
            {
                return $"與{SpecificNumber}位與自己不同陣營的玩家完成交易";
            }
        }

        public CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirement(int questRequirementID, int specificNumber) : base(questRequirementID)
        {
            SpecificNumber = specificNumber;
        }
    }
}
