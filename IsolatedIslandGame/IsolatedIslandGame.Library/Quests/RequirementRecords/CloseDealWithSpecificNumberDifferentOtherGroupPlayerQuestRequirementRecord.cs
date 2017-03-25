using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord : QuestRequirementRecord
    {
        private int closeDealCount;
        public int CloseDealCount
        {
            get { return closeDealCount; }
            private set
            {
                closeDealCount = value;
                QuestRecordFactory.Instance?.UpdateCloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return CloseDealCount >= (Requirement as CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirement).SpecificNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"交易次數： {CloseDealCount}/{(Requirement as CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirement).SpecificNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CloseDealWithSpecificNumberDifferentOtherGroupPlayerQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int closeDealCount) : base(questRequirementRecordID, requirement)
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
                        int otherPlayerID = (transaction.AccepterPlayerID == player.PlayerID) ? transaction.RequesterPlayerID : transaction.AccepterPlayerID;
                        PlayerInformation info;
                        if (PlayerInformationManager.Instance.FindPlayerInformation(otherPlayerID, out info) && info.groupType != player.GroupType)
                        {
                            CloseDealCount = CloseDealCount + 1;
                        }
                    }
                };
            };
        }
    }
}
