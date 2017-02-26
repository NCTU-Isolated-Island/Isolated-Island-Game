using IsolatedIslandGame.Protocol;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirement
    {
        public int QuestRequirementID { get; private set; }
        public abstract QuestRequirementType QuestRequirementType { get; }
        public abstract string Description { get; }

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
