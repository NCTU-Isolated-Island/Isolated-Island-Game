using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class CloseDealWithOutlanderQuestRequirementRecord : QuestRequirementRecord
    {
        private bool hasCloseDealWithOutlander;
        public bool HasCloseDealWithOutlander
        {
            get { return hasCloseDealWithOutlander; }
            private set
            {
                hasCloseDealWithOutlander = value;
                QuestRecordFactory.Instance?.UpdateCloseDealWithOutlanderQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return HasCloseDealWithOutlander;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return (HasCloseDealWithOutlander) ? "已達成" : "尚未達成" ;
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public CloseDealWithOutlanderQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, bool hasCloseDealWithOutlander) : base(questRequirementRecordID, requirement)
        {
            this.hasCloseDealWithOutlander = hasCloseDealWithOutlander;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnTransactionStart += (transaction) =>
            {
                transaction.OnTransactionEnd += (transactionID, isSuccessful) =>
                {
                    if (!IsSufficient && isSuccessful)
                    {
                        int otherPlayerID;
                        if(transaction.RequesterPlayerID == player.PlayerID)
                        {
                            otherPlayerID = transaction.AccepterPlayerID;
                        }
                        else
                        {
                            otherPlayerID = transaction.RequesterPlayerID;
                        }
                        FriendInformation friendInformation;
                        if(!player.FindFriend(otherPlayerID, out friendInformation) || !friendInformation.isConfirmed)
                        {
                            HasCloseDealWithOutlander = true;
                        }
                    }
                };
            };
        }
    }
}
