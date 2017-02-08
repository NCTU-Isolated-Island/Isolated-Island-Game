using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests.Requirements
{
    public class SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement : QuestRequirement
    {
        [MessagePackMember(1)]
        public OceanType SpecificOceanType { get; private set; }
        [MessagePackMember(2)]
        public int RequiredFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameSpecificOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"在{OceanNameMapping.GetOceanName(SpecificOceanType)}海域中，發送訊息給一同位在該海域中的 {RequiredFriendNumber}位 在線好友";
            }
        }

        [MessagePackDeserializationConstructor]
        public SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement() { }
        public SendMessageToDifferentOnlineFriendInTheSameSpecificOceanQuestRequirement(int questRequirementID, OceanType specificOceanType, int requiredFriendNumber) : base(questRequirementID)
        {
            SpecificOceanType = specificOceanType;
            RequiredFriendNumber = requiredFriendNumber;
        }
    }
}
