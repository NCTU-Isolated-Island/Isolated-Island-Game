using IsolatedIslandGame.Library.Quests.Requirements;
using System;

namespace IsolatedIslandGame.Library.Quests.RequirementRecords
{
    public class MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord : QuestRequirementRecord
    {
        private int successfulCount;
        public int SuccessfulCount
        {
            get { return successfulCount; }
            private set
            {
                successfulCount = value;
                QuestRecordFactory.Instance?.UpdateMakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(this);
                onRequirementStatusChange?.Invoke(this);
            }
        }
        public override bool IsSufficient
        {
            get
            {
                return SuccessfulCount >= (Requirement as MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime;
            }
        }

        public override string ProgressStatus
        {
            get
            {
                return $"次數： {SuccessfulCount}/{(Requirement as MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirement).SpecificNumberOfTime}";
            }
        }

        private event Action<QuestRequirementRecord> onRequirementStatusChange;
        public override event Action<QuestRequirementRecord> OnRequirementStatusChange { add { onRequirementStatusChange += value; } remove { onRequirementStatusChange -= value; } }

        public MakeFriendSuccessfulSpecificNumberOfTimeQuestRequirementRecord(int questRequirementRecordID, QuestRequirement requirement, int successfulCount) : base(questRequirementRecordID, requirement)
        {
            this.successfulCount = successfulCount;
        }
        internal override void RegisterObserverEvents(Player player)
        {
            player.OnFriendInformationChange += (changeType, information) =>
            {
                if (!IsSufficient && changeType == Protocol.DataChangeType.Update && information.isConfirmed)
                {
                    SuccessfulCount = SuccessfulCount + 1;
                }
            };
        }
    }
}
