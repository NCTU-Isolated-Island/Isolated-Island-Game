using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberFriendQuestRequirementRecord : QuestRequirementRecord
    {
        private int friendNumber;
        public int FriendNumber
        {
            get { return friendNumber; }
            private set
            {
                friendNumber = value;
                QuestRecordFactory.Instance?.UpdateHaveSpecificNumberFriendQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return FriendNumber >= (Requirement as HaveSpecificNumberFriendQuestRequirement).SpecificFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"好友數量： {FriendNumber}/{(Requirement as HaveSpecificNumberFriendQuestRequirement).SpecificFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberFriendQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int friendNumber) : base(questRequirementRecordID, requirement)
        {
            this.friendNumber = friendNumber;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient)
            {
                FriendNumber = player.FriendCount;
            }
            player.OnFriendInformationChange += (changeType, information) =>
            {
                if (!IsSufficient)
                {
                    FriendNumber = player.FriendCount;
                }
            };
        }
    }
}
