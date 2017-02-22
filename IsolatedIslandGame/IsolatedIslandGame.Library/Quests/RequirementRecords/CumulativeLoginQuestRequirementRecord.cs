using IsolatedIslandGame.Library.Quests.Requirements;
using MsgPack.Serialization;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CumulativeLoginQuestRequirementRecord : QuestRequirementRecord
    {
        public override bool IsSufficient
        {
            get
            {
                return true;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"已登入{(Requirement as CumulativeLoginQuestRequirement).CumulativeLoginCount}天";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        [MessagePackDeserializationConstructor]
        public CumulativeLoginQuestRequirementRecord() { }
        public CumulativeLoginQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement) : base(questRequirementRecordID, requirement)
        {
        }
        internal override void RegisterObserverEvents(Player player)
        {
        }
    }
}
