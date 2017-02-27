using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement : QuestRequirement
    {
        public OceanType SpecificOceanType { get; private set; }
        public int SpecificFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"在{OceanNameMapping.GetOceanName(SpecificOceanType)}海域中，發送訊息給一同位在該海域中的 {SpecificFriendNumber}位 在線好友";
            }
        }

        public SendMessageToSpecificNumberDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(int questRequirementID, OceanType specificOceanType, int specificFriendNumber) : base(questRequirementID)
        {
            SpecificOceanType = specificOceanType;
            SpecificFriendNumber = specificFriendNumber;
        }
    }
}
