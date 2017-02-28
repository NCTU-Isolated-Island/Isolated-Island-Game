using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class HaveSpecificNumberFriendQuestRequirementRecord : QuestRequirementRecord
    {
        private int friendCount;
        public int FriendCount
        {
            get { return friendCount; }
            private set
            {
                friendCount = value;
                QuestRecordFactory.Instance?.UpdateHaveSpecificNumberFriendQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return FriendCount >= (Requirement as HaveSpecificNumberFriendQuestRequirement).SpecificFriendNumber;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"好友數量： {FriendCount}/{(Requirement as HaveSpecificNumberFriendQuestRequirement).SpecificFriendNumber}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public HaveSpecificNumberFriendQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int friendCount) : base(questRequirementRecordID, requirement)
        {
            this.friendCount = friendCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            if (!IsSufficient)
            {
                FriendCount = player.FriendCount;
            }
            player.OnFriendInformationChange += (changeType, information) =>
            {
                if (!IsSufficient)
                {
                    FriendCount = player.FriendCount;
                }
            };
        }
    }
}
