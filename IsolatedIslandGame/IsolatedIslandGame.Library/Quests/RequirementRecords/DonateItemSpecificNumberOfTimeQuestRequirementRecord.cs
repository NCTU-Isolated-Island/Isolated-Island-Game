using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class DonateItemSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int donatedItemCount;
        public int DonatedItemCount
        {
            get { return donatedItemCount; }
            private set
            {
                donatedItemCount = value;
                QuestRecordFactory.Instance?.UpdateDonateItemSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return DonatedItemCount >= (Requirement as DonateItemSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"次數： {DonatedItemCount}/{(Requirement as DonateItemSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public DonateItemSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int donatedItemCount) : base(questRequirementRecordID, requirement)
        {
            this.donatedItemCount = donatedItemCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnDonateItem += () => 
            {
                if (!IsSufficient)
                {
                    DonatedItemCount = DonatedItemCount + 1;
                }
            };
        }
    }
}
