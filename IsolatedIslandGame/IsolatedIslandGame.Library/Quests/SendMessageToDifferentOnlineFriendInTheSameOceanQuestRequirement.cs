using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public class SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement : QuestRequirement
    {
        [MessagePackMember(1)]
        public int RequiredFriendNumber { get; private set; }
        public override QuestRequirementType QuestRequirementType
        {
            get
            {
                return QuestRequirementType.SendMessageToDifferentOnlineFriendInTheSameOcean;
            }
        }
        public override string Description
        {
            get
            {
                return $"發送訊息給 {RequiredFriendNumber}位 位在海域的好友";
            }
        }

        [MessagePackDeserializationConstructor]
        public SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement() { }
        public SendMessageToDifferentOnlineFriendInTheSameOceanQuestRequirement(int questRequirementID, int requiredFriendNumber) : base(questRequirementID)
        {
            RequiredFriendNumber = requiredFriendNumber;
        }

        public override bool CreateRequirementRecord(int questRecordID, int playerID, out QuestRequirementRecord record)
        {
            return QuestRecordFactory.Instance.CreateSendMessageToDifferentOnlineFriendQuestRequirementRecord(questRecordID, playerID, this, out record);
        }
    }
}
