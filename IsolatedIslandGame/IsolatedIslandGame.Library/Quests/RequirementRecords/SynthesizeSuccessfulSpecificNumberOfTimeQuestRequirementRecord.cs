using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int successfulNumberOfTime;
        public int SuccessfulNumberOfTime
        {
            get { return successfulNumberOfTime; }
            private set
            {
                successfulNumberOfTime = value;
                QuestRecordFactory.Instance?.UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return SuccessfulNumberOfTime >= (Requirement as SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificSuccessfulNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"成功次數： {SuccessfulNumberOfTime}/{(Requirement as SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificSuccessfulNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int successfulNumberOfTime) : base(questRequirementRecordID, requirement)
        {
            this.successfulNumberOfTime = successfulNumberOfTime;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnSynthesizeResultGenerated += (isSuccessful, resultBlueprint) =>
            {
                if (!IsSufficient && isSuccessful)
                {
                    SuccessfulNumberOfTime = SuccessfulNumberOfTime + 1;
                }
            };
        }
    }
}
