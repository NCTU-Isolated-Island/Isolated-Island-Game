using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirement : QuestRequirement
    {
        public int SpecificNumber { get; private set; }
        public GroupType SpecificGroupType { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToSpecificNumberSpecificGroupDifferentPlayer;
            }
        }
        public override string Description
        {
            get
            {
                return $"發送訊息給 {SpecificNumber}位 {GroupNameMapping.GetGroupName(SpecificGroupType)}陣營的玩家";
            }
        }

        public SendMessageToSpecificNumberSpecificGroupDifferentPlayerQuestRequirement(int questRequirementID, int specificNumber, GroupType specificGroupType) : base(questRequirementID)
        {
            SpecificGroupType = specificGroupType;
            SpecificNumber = specificNumber;
        }
    }
}
