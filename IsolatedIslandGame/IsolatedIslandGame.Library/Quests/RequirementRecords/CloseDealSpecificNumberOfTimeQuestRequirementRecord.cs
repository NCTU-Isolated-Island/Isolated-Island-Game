using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CloseDealSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int closeDealCount;
        public int CloseDealCount
        {
            get { return closeDealCount; }
            private set
            {
                closeDealCount = value;
                QuestRecordFactory.Instance?.UpdateCloseDealSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return CloseDealCount >= (Requirement as CloseDealSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"交易次數： {CloseDealCount}/{(Requirement as CloseDealSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CloseDealSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int closeDealCount) : base(questRequirementRecordID, requirement)
        {
            this.closeDealCount = closeDealCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnTransactionStart += (transaction) =>
            {
                transaction.OnTransactionEnd += (transactionID, isSuccessful) =>
                {
                    if (!IsSufficient && isSuccessful)
                    {
                        CloseDealCount = CloseDealCount + 1;
                    }
                };
            };
        }
    }
}
