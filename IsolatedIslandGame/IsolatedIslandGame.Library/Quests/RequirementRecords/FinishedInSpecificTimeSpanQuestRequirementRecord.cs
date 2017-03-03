using IsolatedIslandGame.Library.Quests.Requirements;
using System;
using System.Linq;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class FinishedInSpecificTimeSpanQuestRequirementRecord : QuestRequirementRecord
    {
        public DateTime StartTime { get; private set; }
        private bool isExpired;
        public bool IsExpired
        {
            get { return isExpired; }
            private set
            {
                isExpired = value;
                QuestRecordFactory.Instance?.UpdateFinishedInSpecificTimeSpanQuestRequirementRecord(this);
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
                return (IsSufficient) ? $"到期時間： {StartTime + (Requirement as FinishedInSpecificTimeSpanQuestRequirement).SpecificTimeSpan}" : "已過期";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public FinishedInSpecificTimeSpanQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, DateTime startTime, bool isExpired) : base(questRequirementRecordID, requirement)
        {
            StartTime = startTime;
            this.isExpired = isExpired;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsExpired && player.QuestRecords.Any(x => !x.IsFinished && x.RequirementRecords.Any(y => y.QuestRequirementRecordID == QuestRequirementRecordID)))
            {
                Scheduler.Instance.AddTask(StartTime + (Requirement as FinishedInSpecificTimeSpanQuestRequirement).SpecificTimeSpan, () =>
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
