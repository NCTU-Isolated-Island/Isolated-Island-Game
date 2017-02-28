using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int successfulCount;
        public int SuccessfulCount
        {
            get { return successfulCount; }
            private set
            {
                successfulCount = value;
                QuestRecordFactory.Instance?.UpdateSynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return SuccessfulCount >= (Requirement as SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificSuccessfulNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"成功次數： {SuccessfulCount}/{(Requirement as SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificSuccessfulNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public SynthesizeSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int successfulCount) : base(questRequirementRecordID, requirement)
        {
            this.successfulCount = successfulCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnSynthesizeResultGenerated += (isSuccessful, resultBlueprint) =>
            {
                if (!IsSufficient && isSuccessful)
                {
                    SuccessfulCount = SuccessfulCount + 1;
                }
            };
        }
    }
}
