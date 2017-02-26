using IsolatedIslandGame.Library.Quests.Requirements;
using MsgPack.Serialization;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CumulativeLoginQuestRequirementRecord : QuestRequirementRecord
    {
        [MessagePackMember(3)]
        private bool hasAchievedCumulativeLoginCount;
        public bool HasAchievedCumulativeLoginCount
        {
            get { return hasAchievedCumulativeLoginCount; }
            private set
            {
                hasAchievedCumulativeLoginCount = value;
                if (hasAchievedCumulativeLoginCount == true)
                {
                    //QuestRecordFactory.Instance?.MarkCumulativeLoginQuestRequirementRecordHasAchievedCumulativeLoginCount(QuestRequirementRecordID);
                }
            }
        }

        public override bool IsSufficient
        {
            get
            {
                return HasAchievedCumulativeLoginCount;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                if(HasAchievedCumulativeLoginCount)
                {
                    return "已達到所需的登入次數";
                }
                else
                {
                    return "尚未達到所需的登入次數";
                }
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        [MessagePackDeserializationConstructor]
        public CumulativeLoginQuestRequirementRecord() { }
        public CumulativeLoginQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasAchievedCumulativeLoginCount) : base(questRequirementRecordID, requirement)
        {
            this.hasAchievedCumulativeLoginCount = hasAchievedCumulativeLoginCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient && player.CumulativeLoginCount >= (Requirement as CumulativeLoginQuestRequirement).CumulativeLoginCount)
            {
                HasAchievedCumulativeLoginCount = true;
                onRequirementStatusChange?.Invoke(this);
            }
        }
    }
}
