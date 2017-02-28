using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CumulativeLoginSpecificDayQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasAchievedCumulativeLoginDayCount;
        public bool HasAchievedCumulativeLoginDayCount
        {
            get { return hasAchievedCumulativeLoginDayCount; }
            private set
            {
                hasAchievedCumulativeLoginDayCount = value;
                QuestRecordFactory.Instance?.UpdateCumulativeLoginSpecificDayQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasAchievedCumulativeLoginDayCount;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasAchievedCumulativeLoginDayCount) ? "已達到所需的登入次數" : "尚未達到所需的登入次數";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CumulativeLoginSpecificDayQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasAchievedCumulativeLoginDayCount) : base(questRequirementRecordID, requirement)
        {
            this.hasAchievedCumulativeLoginDayCount = hasAchievedCumulativeLoginDayCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient && player.CumulativeLoginCount >= (Requirement as CumulativeLoginSpecificDayQuestRequirement).SpecificDayCount)
            {
                HasAchievedCumulativeLoginDayCount = true;
            }
        }
    }
}
