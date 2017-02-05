using System;

namespace IsolatedIslandGame.Library.Quests
{
    public abstract class QuestRequirementRecord
    {
        public int QuestRequirementRecordID { get; private set; }
        protected Player player;
        public QuestRequirement Requirement { get; private set; }
        public abstract string ProgressStatus { get; }
        public abstract bool IsSufficient { get; }
        public abstract event Action<QuestRequirementRecord> OnRequirementStatusChange;

        protected QuestRequirementRecord(int questRequirementRecordID, Player player, QuestRequirement requirement)
        {
            QuestRequirementRecordID = questRequirementRecordID;
            this.player = player;
            Requirement = requirement;
        }
    }
}
