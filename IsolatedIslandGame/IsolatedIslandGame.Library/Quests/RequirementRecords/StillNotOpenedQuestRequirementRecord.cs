using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class StillNotOpenedQuestRequirementRecord : QuestRequirementRecord
    {
        public override bool IsSufficient
        {
            get
            {
                return false;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return "尚未開放";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public StillNotOpenedQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement) : base(questRequirementRecordID, requirement)
        {

        }
        internal override void RegisterObserverEvents(Player player)
        {
        }
    }
}
