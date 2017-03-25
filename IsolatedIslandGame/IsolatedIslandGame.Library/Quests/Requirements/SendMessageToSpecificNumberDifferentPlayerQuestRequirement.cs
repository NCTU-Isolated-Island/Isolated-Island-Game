using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMessageToSpecificNumberDifferentPlayerQuestRequirement : QuestRequirement
    {
        public int SpecificNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToSpecificNumberDifferentPlayer;
            }
        }
        public override string Description
        {
            get
            {
                return $"發送訊息給 {SpecificNumber}位 玩家";
            }
        }

        public SendMessageToSpecificNumberDifferentPlayerQuestRequirement(int questRequirementID, int specificNumber) : base(questRequirementID)
        {
            SpecificNumber = specificNumber;
        }
    }
}
