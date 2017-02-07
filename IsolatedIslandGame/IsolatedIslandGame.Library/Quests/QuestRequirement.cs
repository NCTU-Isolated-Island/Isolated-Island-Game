using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        [MessagePackMember(0)]
        public int QuestRequirementID { get; private set; }
        public abstract QuestRequirementType QuestRequirementType { get; }
        public abstract string Description { get; }

        [MessagePackDeserializationConstructor]
        public QuestRequirement() { }
        protected QuestRequirement(int questRequirementID)
        {
            QuestRequirementID = questRequirementID;
        }

        public bool CreateRequirementRecord(int questRecordID, int playerID, out QuestRequirementRecord record)
        {
            return QuestRecordFactory.Instance.CreateQuestRequirementRecord(questRecordID, playerID, this, out record);
        }
    }
}
