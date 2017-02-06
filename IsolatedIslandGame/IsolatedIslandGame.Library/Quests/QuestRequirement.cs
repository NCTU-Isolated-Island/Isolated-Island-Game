using IsolatedIslandGame.Protocol;
using MsgPack.Serialization;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        public int QuestRequirementID { get; private set; }
        public abstract QuestRequirementType QuestRequirementType { get; }
        public abstract string Description { get; }

        [MessagePackDeserializationConstructor]
        public QuestRequirement() { }
        protected QuestRequirement(int questRequirementID)
        {
            QuestRequirementID = questRequirementID;
        }

        public abstract bool CreateRequirementRecord(int questRecordID, Player player, out QuestRequirementRecord record);
    }
}
