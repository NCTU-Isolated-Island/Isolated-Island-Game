using IsolatedIslandGame.Library.Quests.Requirements;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class FinishedBeforeSpecificTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private bool isExpired;
        public bool IsExpired
        {
            get { return isExpired; }
            private set
            {
                isExpired = value;
                QuestRecordFactory.Instance?.UpdateFinishedBeforeSpecificTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return !isExpired;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (isExpired) ? "已過期" : $"未到期";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public FinishedBeforeSpecificTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool isExpired) : base(questRequirementRecordID, requirement)
        {
            this.isExpired = isExpired;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if(!IsExpired && player.QuestRecords.Any(x => !x.IsFinished && x.RequirementRecords.Any(y => y.QuestRequirementRecordID == QuestRequirementRecordID)))
            {
                Scheduler.Instance.AddTask((Requirement as FinishedBeforeSpecificTimeQuestRequirement).SpecificTime, () =>
                {
                    if (player.QuestRecords.Any(x => !x.IsFinished && x.RequirementRecords.Any(y => y.QuestRequirementRecordID == QuestRequirementRecordID)))
                    {
                        IsExpired = true;
                    }
                });
            }
        }
    }
}
