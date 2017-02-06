using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendQuestRequirement : QuestRequirement
    {
        public int RequiredOnlinedFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToDifferentOnlineFriend;
            }
        }
        public override string Description
        {
            get
            {
                return $"發送訊息給{RequiredOnlinedFriendNumber}位不同的好友";
            }
        }

        [MessagePackDeserializationConstructor]
        public SendMessageToDifferentOnlineFriendQuestRequirement() { }
        public SendMessageToDifferentOnlineFriendQuestRequirement(int questRequirementID, int requiredOnlinedFriendNumber) : base(questRequirementID)
        {
            RequiredOnlinedFriendNumber = requiredOnlinedFriendNumber;
        }

        public override bool CreateRequirementRecord(int questRecordID, Player player, out QuestRequirementRecord record)
        {
            return QuestRecordFactory.Instance.CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(questRecordID, player, this, out record);
        }
    }
}
